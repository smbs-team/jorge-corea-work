// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useEffect, useState } from 'react';
import {
  Box,
  createStyles,
  makeStyles,
  useTheme,
  Theme,
  Typography,
  Switch,
} from '@material-ui/core';
import {
  ColorPicker,
  CustomButton,
  CustomPopover,
  CustomTabs,
  CustomTextField,
  SimpleDropDown,
} from '@ptas/react-ui-library';
import { RGBA_BLACK, RGBA_WHITE } from 'appConstants';
import {
  FONT_STYLES,
  FONT_SIZES,
  FONT_STYLE_NAME,
  OPTION_COLORS,
  INITIAL_BUBBLE_COLOR,
} from './Constants';
import { dropdownItemMap } from './Helpers';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    popover: {
      width: '465px',
      overflow: 'hidden',
      padding: theme.spacing(2),
      paddingTop: '35px',
    },
    buttonWrapper: {
      display: 'flex',
      flexDirection: 'row',
      justifyContent: 'flex-end',
      paddingTop: '15px',
    },
    groupTitle: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
    },
    colorField: {
      width: 130,
    },
    marginRight: {
      marginRight: theme.spacing(2),
    },
    fontSizeDropDown: {
      marginRight: theme.spacing(2),
      width: '100px',
    },
    row: {
      display: 'flex',
      flexDirection: 'row',
      marginBottom: '8px',
    },
    switchChecked: {
      color: theme.ptas.colors.utility.selection,
    },
    switchTrack: {
      backgroundColor: theme.ptas.colors.utility.selection,
    },
    rowFlexBox: {
      display: 'flex',
      alignItems: 'center',
    },
    colorSwitch: {
      width: '155px',
      margin: '0 10px',
    },
    outlineWidthInput: {
      width: '95px',
    },
    bubbleTextInput: {
      width: '100%',
    },
    switchLabel: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.formLabel.fontSize,
      fontWeight: theme.ptas.typography.formLabel.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
      paddingTop: '6px',
    },
  })
);

export interface Settings {
  id: string;
  text: string;
  textColor: string;
  fontWeight: string;
  fontStyle: string;
  fontSize: string;
  backgroundColor: string;
  borderColor: string;
  borderWidth: number;
}

export interface BubbleSettingsProps {
  anchorEl?: HTMLElement | null | undefined;
  close?: () => void;
  settings?: Settings;
  updateBubbleSettings?: (settings: Settings) => void;
}

type BubbleColors = {
  fill: string;
  outline: string;
};

const BubbleSettings = (
  props: PropsWithChildren<BubbleSettingsProps>
): JSX.Element => {
  const {
    text,
    textColor,
    fontStyle: fontStyleProp,
    fontSize: fontSizeProp,
    fontWeight,
    borderWidth,
    borderColor,
    backgroundColor,
  } = props.settings || {};
  const classes = useStyles(useTheme());
  const [bubbleColor, setBubbleColor] = useState<BubbleColors>({
    outline: borderColor ?? INITIAL_BUBBLE_COLOR.outline,
    fill: backgroundColor ?? INITIAL_BUBBLE_COLOR.fill,
  });
  const [bubbleText, setBubbleText] = useState<string>(text ?? '');
  const [bubbleColorTab, setBubbleColorTab] = useState<number>(0);
  const [color, setColor] = useState<string>(textColor ?? RGBA_BLACK);
  const [fontStyle, setFontStyle] = useState<string>(
    fontStyleProp ?? FONT_STYLE_NAME.normal
  );
  const [fontSize, setFontSize] = useState<string>(fontSizeProp ?? '12pt');
  const [isBold, setIsBold] = useState<boolean>(fontWeight === 'bold');
  const [outlineWidth, setOutlineWidth] = useState<number>(borderWidth ?? 1);

  useEffect(() => {
    if (props.anchorEl) {
      setBubbleColorTab(0);
      setBubbleColor({
        outline: borderColor ?? INITIAL_BUBBLE_COLOR.outline,
        fill: backgroundColor ?? INITIAL_BUBBLE_COLOR.fill,
      });
      setBubbleText(text ?? '');
      setColor(textColor ?? RGBA_BLACK);
      setFontStyle(fontStyleProp ?? FONT_STYLE_NAME.normal);
      setFontSize(fontSizeProp ?? '12pt');
      setIsBold(fontWeight === 'bold');
      setOutlineWidth(borderWidth ?? 1);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.anchorEl]);

  const handleTextChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    setBubbleText(e.target.value);
  };

  const getColor = (): string => {
    return (
      (bubbleColorTab === 0 ? bubbleColor.fill : bubbleColor.outline) ??
      RGBA_WHITE
    );
  };

  const changeBubbleColor = (color: string): void => {
    setBubbleColor((bc) => {
      const updateColor =
        bubbleColorTab === 0 ? { fill: color } : { outline: color };
      return {
        ...bc,
        ...updateColor,
      };
    });
  };

  const outlineWidthChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const val = parseInt(e.target.value);
    setOutlineWidth(val);
  };

  const handleUpdateBubbleSettings = (): void => {
    const { updateBubbleSettings, settings, close } = props;
    if (!settings || !updateBubbleSettings) return;
    updateBubbleSettings({
      id: settings.id,
      backgroundColor: bubbleColor.fill,
      borderColor: bubbleColor.outline,
      borderWidth: outlineWidth,
      fontSize,
      fontStyle,
      fontWeight: isBold ? 'bold' : 'normal',
      text: bubbleText,
      textColor: color,
    });
    close?.();
  };

  return (
    <CustomPopover
      anchorEl={props.anchorEl}
      onClose={(): void => {
        const { close } = props;
        close?.();
      }}
      border
      showCloseButton
    >
      <Box className={classes.popover}>
        <div className={classes.row}>
          <Typography className={classes.groupTitle}>Text</Typography>
        </div>
        <div className={classes.row}>
          <CustomTextField
            label="text"
            type="string"
            value={bubbleText}
            onChange={handleTextChange}
            multiline
            rows={3}
            classes={{
              root: classes.bubbleTextInput,
            }}
          />
        </div>
        <div className={classes.row}>
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
          <SimpleDropDown
            classes={{
              root: classes.fontSizeDropDown,
            }}
            label="Font style"
            items={dropdownItemMap(FONT_STYLES)}
            onSelected={(item): void => {
              setFontStyle(item.value as string);
            }}
            value={`${fontStyle ?? ''}`}
          />
          <SimpleDropDown
            classes={{
              root: classes.fontSizeDropDown,
            }}
            label="Font size"
            items={dropdownItemMap(FONT_SIZES)}
            onSelected={(item): void => {
              setFontSize(item.value as string);
            }}
            value={`${fontSize ?? ''}`}
          />
        </div>
        <div className={classes.row}>
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
        </div>
        <div className={classes.row}>
          <Typography className={classes.groupTitle}>Bubble</Typography>
        </div>
        <div className={classes.row}>
          <Box className={classes.rowFlexBox}>
            <ColorPicker
              onChangeComplete={(color): void => {
                changeBubbleColor(color.rgbaStr);
              }}
              rgbColor={getColor()}
              showHexInput
              label="Color"
              classes={{
                textField: classes.colorField,
              }}
            />
            <Box className={classes.rowFlexBox}>
              <CustomTabs
                switchVariant
                items={OPTION_COLORS}
                onSelected={(e: number): void => {
                  setBubbleColorTab(e);
                }}
                classes={{ root: classes.colorSwitch }}
              />
            </Box>
            <CustomTextField
              label="Outline width (px)"
              type="number"
              value={outlineWidth}
              className={classes.outlineWidthInput}
              onChange={outlineWidthChange}
            />
          </Box>
        </div>
        <div className={classes.buttonWrapper}>
          <span>
            <CustomButton fullyRounded onClick={handleUpdateBubbleSettings}>
              Save
            </CustomButton>
          </span>
        </div>
      </Box>
    </CustomPopover>
  );
};

export default BubbleSettings;
