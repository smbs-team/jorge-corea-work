// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect } from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import Chip from "../Chip";
import { useList } from "react-use";
import ScrollMenu from "react-horizontal-scrolling-menu";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  chipData: ChipDataType[];
  onChipClick?: (chip: ChipDataType) => void;
}

export interface ChipDataType {
  id: React.ReactText;
  label: string;
  isSelected?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      backgroundColor: "#afafaf"
    },
    chip: {
      margin: theme.spacing(0.5, 0.25, 0.5, 0.25)
    },
    scrollWrapper: {
      width: "100%",
      padding: theme.spacing(0, 0.75, 0, 0.75)
    }
  });

/**
 * ChipBar
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ChipBar(props: Props): JSX.Element {
  const { classes } = props;
  const [selectedChips, selectedChipsMethods] = useList<React.ReactText>([]);

  useEffect(() => {
    props.chipData.forEach((c) => {
      if (c.isSelected) {
        if (selectedChips.includes(c.id)) return;
        selectedChipsMethods.push(c.id);
      } else {
        if (selectedChips.includes(c.id))
          selectedChipsMethods.removeAt(
            selectedChips.findIndex((s) => s === c.id)
          );
      }
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.chipData]);

  const handleChipOnClick = (chipId: React.ReactText) => {
    const chip = props.chipData.find(
      (c) => c.id.toString() === chipId.toString()
    );
    if (!chip) return;
    const { id, label } = chip;
    if (selectedChips.includes(id)) {
      selectedChipsMethods.removeAt(selectedChips.findIndex((s) => s === id));
      props.onChipClick?.({ id: id, isSelected: false, label: label });
    } else {
      if (selectedChips.length === 5) return;
      selectedChipsMethods.push(id);
      props.onChipClick?.({ id: id, isSelected: true, label: label });
    }
  };

  return (
    <ScrollMenu
      scrollBy={2}
      alignCenter={false}
      inertiaScrolling
      menuClass={classes.root}
      wrapperClass={classes.scrollWrapper}
      hideArrows
      onSelect={(key) => (key ? handleChipOnClick(key) : null)}
      data={props.chipData.map((c) => (
        <Chip
          key={c.id}
          chipId={c.id}
          label={c.label}
          isSelected={selectedChips.includes(c.id)}
          classes={{ root: classes.chip }}
          ButtonBaseProps={{
            disableRipple: selectedChips.length === 5
          }}
        />
      ))}
    />
  );
}

export default withStyles(useStyles)(ChipBar);
