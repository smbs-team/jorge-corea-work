// SectionUses.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import {
  CustomChip,
  CustomPopover,
  GenericListSearch,
} from '@ptas/react-ui-library';
import { GridSection } from 'components/shared';
import { useRef, useState } from 'react';
import useSectionUseStore, { SectionUseList } from 'stores/useSectionUseStore';
import CreateIcon from '@material-ui/icons/Create';
import usePageManagerStore from 'stores/usePageManagerStore';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';

interface Props {
  showRentableArea?: boolean;
  width?: string;
}

const useStyles = makeStyles((theme) => ({
  root: {
    flexDirection: 'row',
  },
  chipList: {
    overflowY: 'auto',
    marginRight: 14,
  },
  sectionWrapper: {
    width: (props: Props) => props.width ?? undefined,
  },
  popoverPaper: {
    borderRadius: 9,
    padding: theme.spacing(1),
    paddingTop: 0,
  },
  customChip: {
    width: 'fit-content',
    margin: theme.spacing(0.5),
  },
}));

function SectionUses(props: Props): JSX.Element {
  const classes = useStyles(props);
  const store = useSectionUseStore();
  const [isLoadComplete, setIsLoadComplete] = useState<boolean>(false);
  const pageManagerStore = usePageManagerStore();
  const [popoverEl, setPopoverEl] = useState<HTMLElement | null>(null);
  const inputEl = useRef<HTMLDivElement>(null);

  const onEditSectionUseClick = async (
    event: React.MouseEvent<HTMLButtonElement>
  ): Promise<void> => {
    const savedEventTarget = event.currentTarget;
    if (!isLoadComplete) {
      await store.getSectionUses();
      setIsLoadComplete(true);
    }
    setPopoverEl(savedEventTarget);
  };

  return (
    <GridSection
      title="Selected Section Uses"
      icons={[
        {
          icon:
            pageManagerStore.selectedSectionUses.length !== 0 ? (
              <CreateIcon />
            ) : (
              <AddCircleOutlineIcon />
            ),
          text:
            pageManagerStore.selectedSectionUses.length !== 0
              ? 'Edit Section Use List'
              : 'Add Section Uses',
          onClick: onEditSectionUseClick,
        },
      ]}
      classes={{
        sectionContentWrapper: classes.root,
        sectionWrapper: classes.sectionWrapper,
      }}
      isLoading={store.isLoading}
    >
      <div className={classes.chipList}>
        {pageManagerStore.selectedSectionUses.map((item, i) => (
          <CustomChip
            key={i}
            label={item.value}
            onDelete={() => pageManagerStore.addSectionUse(item)}
            className={classes.customChip}
          />
        ))}
      </div>
      <CustomPopover
        anchorEl={popoverEl}
        onClose={(): void => {
          setPopoverEl(null);
        }}
        anchorOrigin={{
          vertical: 'center',
          horizontal: 'right',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        classes={{ paper: classes.popoverPaper }}
        TransitionProps={{ onEntered: () => inputEl.current?.focus() }}
      >
        <GenericListSearch<SectionUseList>
          data={store.sectionUseList.map((s) => {
            return {
              key: s.id as string,
              value: s.name,
              description: s.description,
            };
          })}
          onClick={(selectedItem) =>
            pageManagerStore.addSectionUse(selectedItem)
          }
          selectedData={pageManagerStore.selectedSectionUses.flatMap(
            (s) => s.key
          )}
          textFieldLabel="Find section name, page or code"
          TextFieldProps={{ inputRef: inputEl }}
        />
      </CustomPopover>
    </GridSection>
  );
}

export default SectionUses;
