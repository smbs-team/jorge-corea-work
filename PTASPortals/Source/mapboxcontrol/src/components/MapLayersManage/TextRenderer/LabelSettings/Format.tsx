// Format.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect, useState } from 'react';
import {
  ColorPicker,
  CustomTabs,
  CustomTextField,
  SimpleDropDown,
} from '@ptas/react-ui-library';
import {
  createStyles,
  useTheme,
  Theme,
  makeStyles,
  Box,
  Switch,
} from '@material-ui/core';
import { TagsManageContext } from '../TagsManageContext';
import { ClassTagLabel } from '../typing';
import { RGBA_BLACK, RGBA_WHITE } from 'appConstants';
import { useDebounce } from 'react-use';
import { HomeContext } from 'contexts';

const fontSizes = new Array(Math.floor(30 / 2))
  .fill(0)
  .map((v, k) => (k + 1) * 2 + 'pt');

const HORIZONTAL_ALIGN_ITEMS = ['Left', 'Center', 'Right'];
const VERTICAL_ALIGN_ITEMS = ['Top', 'Middle', 'Bottom'];

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    rowFlexBox: {
      display: 'flex',
      padding: '10px',
    },
    colorField: {
      width: 130,
    },
    marginRight: {
      marginRight: theme.spacing(4),
    },
    fontFamilyDropDown: {
      marginRight: theme.spacing(2),
      width: '100px',
    },
    box: {
      alignItems: 'center',
      display: 'flex',
    },
    switchChecked: {
      color: theme.ptas.colors.utility.selection,
    },
    switchTrack: {
      backgroundColor: theme.ptas.colors.utility.selection,
    },
    textBox: {
      width: '100px',
    },
    switchLabel: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
    },
  })
);

function Format(): JSX.Element | null {
  const classes = useStyles(useTheme());
  const { currentLayer } = useContext(HomeContext);
  const { selectedLabel, setSelectedLabel } = useContext(TagsManageContext);
  const [color, setColor] = useState<string>(RGBA_WHITE);
  const [background, setBackground] = useState<string>(RGBA_BLACK);
  const [alignV, setAlignV] = useState<string>(VERTICAL_ALIGN_ITEMS[1]);
  const [alignH, setAlignH] = useState<string>(HORIZONTAL_ALIGN_ITEMS[1]);
  const [fontSize, setFontSize] = useState<string>('12pt');
  const [isBold, setIsBold] = useState<boolean>(false);
  const [padding, setPadding] = useState<number>(0);

  useEffect(() => {
    if (selectedLabel) {
      const {
        color: textColor,
        backgroundColor,
        horizontalAlign,
        verticalAlign,
        fontSize: fontSizeSaved,
        isBoldText,
        padding: paddingText,
      } = selectedLabel;
      setColor(textColor);
      setBackground(backgroundColor);
      setAlignH(horizontalAlign);
      setAlignV(verticalAlign);
      setFontSize(fontSizeSaved);
      setIsBold(!!isBoldText);
      setPadding(paddingText);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedLabel?.id]);

  useDebounce(
    (): void => {
      if (!selectedLabel) return;
      const labelUpdated: ClassTagLabel = {
        ...selectedLabel,
        color,
        backgroundColor: background,
        horizontalAlign: alignH,
        verticalAlign: alignV,
        fontSize,
        isBoldText: isBold,
        padding,
      };
      setSelectedLabel(labelUpdated);
    },
    100,
    [color, background, alignH, alignV, fontSize, isBold, padding]
  );

  const textCompare = (textA: string, textB: string): boolean => {
    return (
      `${textA}`.localeCompare(`${textB}`, undefined, {
        sensitivity: 'accent',
      }) === 0
    );
  };

  const getArrayIndex = (options: string[], label: string): number => {
    return options.findIndex((opt) => textCompare(opt, label)) ?? 0;
  };

  const getArrayLabel = (options: string[], index: number): string => {
    return (options ?? [])[index] ?? '';
  };

  return currentLayer ? (
    <div>
      <Box className={classes.rowFlexBox}>
        <ColorPicker
          onChangeComplete={(color): void => {
            setColor(color.rgbaStr);
          }}
          rgbColor={color}
          showHexInput
          label="Color"
          classes={{
            root: classes.marginRight,
            textField: classes.colorField,
          }}
        />
        <ColorPicker
          onChangeComplete={(color): void => {
            setBackground(color.rgbaStr);
          }}
          rgbColor={background}
          showHexInput
          label="Background"
          classes={{
            root: classes.marginRight,
            textField: classes.colorField,
          }}
        />
      </Box>
      <Box className={classes.rowFlexBox}>
        <Box className={classes.box}>
          <SimpleDropDown
            classes={{
              root: classes.fontFamilyDropDown,
            }}
            label="Font size"
            items={
              fontSizes?.map((item) => ({ label: item, value: item })) || []
            }
            onSelected={(item): void => {
              setFontSize(item.value as string);
            }}
            value={`${fontSize ?? ''}`}
          />
          <CustomTextField
            className={classes.textBox}
            label="Padding (px)"
            type="number"
            value={padding}
            onChange={(e): void => {
              setPadding(parseInt(e.currentTarget.value));
            }}
          />
          <Switch
            onChange={(event): void => {
              setIsBold(event.currentTarget.checked);
            }}
            classes={{
              checked: classes.switchChecked,
              track: classes.switchTrack,
            }}
            color="primary"
            checked={isBold}
          />
          <span className={classes.switchLabel}>Bold</span>
        </Box>
      </Box>
      <Box className={classes.rowFlexBox}>
        <CustomTabs
          switchVariant
          selectedIndex={getArrayIndex(HORIZONTAL_ALIGN_ITEMS, alignH)}
          items={HORIZONTAL_ALIGN_ITEMS}
          onSelected={(e: number): void => {
            if (currentLayer.rendererRules.layer.type === 'circle' && e !== 1) {
              setAlignV(VERTICAL_ALIGN_ITEMS[1]);
            }
            const labelFound = getArrayLabel(HORIZONTAL_ALIGN_ITEMS, e);
            setAlignH(labelFound);
          }}
        />
      </Box>
      <Box className={classes.rowFlexBox}>
        <CustomTabs
          switchVariant
          selectedIndex={getArrayIndex(VERTICAL_ALIGN_ITEMS, alignV)}
          items={VERTICAL_ALIGN_ITEMS}
          onSelected={(e: number): void => {
            if (currentLayer.rendererRules.layer.type === 'circle' && e !== 1) {
              setAlignH(HORIZONTAL_ALIGN_ITEMS[1]);
            }
            const labelFound = getArrayLabel(VERTICAL_ALIGN_ITEMS, e);
            setAlignV(labelFound);
          }}
        />
      </Box>
    </div>
  ) : null;
}

export default Format;
