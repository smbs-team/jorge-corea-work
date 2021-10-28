// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme,
  ChipProps
} from "@material-ui/core";
import CustomChip from "../CustomChip";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  chipData: ChipItem[];
  ChipProps?: ChipProps;
  onChange?: (deletedChip: ChipItem) => void;
  onClick?: (
    e: React.MouseEvent<HTMLDivElement, MouseEvent>,
    item: ChipItem
  ) => void;
}

export interface ChipItem {
  key: string | number;
  label: string;
  isDeletable?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      width: "fit-content"
    },
    chip: {
      margin: theme.spacing(0.5)
    }
  });

/**
 * ChipContainer
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ChipContainer(props: Props): JSX.Element {
  const { classes } = props;

  const [chipData, setChipData] = useState(props.chipData);
  const [selectedChips, setSelectedChips] = useState<number[]>([]);

  useEffect(() => {
    setChipData(props.chipData);
  }, [props.chipData]);

  const handleDelete = (chipToDelete: ChipItem) => () => {
    setChipData((chips) =>
      chips.filter((chip) => chip.key !== chipToDelete.key)
    );
    props.onChange && props.onChange(chipToDelete);
  };

  const handleClick = (
    e: React.MouseEvent<HTMLDivElement, MouseEvent>,
    item: ChipItem
  ) => {
    props.onClick?.(e, item);
    if (selectedChips.includes(item.key as number)) {
      setSelectedChips(selectedChips.filter((s) => s !== item.key));
    } else {
      setSelectedChips([...selectedChips, item.key as number]);
    }
  };

  return (
    <Box className={classes.root}>
      {chipData.map((data) => {
        return (
          <CustomChip
            key={data.key}
            label={data.label}
            onDelete={data.isDeletable ? handleDelete(data) : undefined}
            className={classes.chip}
            onClick={(e) => handleClick(e, data)}
            isSelected={selectedChips.includes(data.key as number)}
            {...props.ChipProps}
          />
        );
      })}
    </Box>
  );
}

export default withStyles(useStyles)(ChipContainer);
