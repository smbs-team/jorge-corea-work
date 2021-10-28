// BusinessForm.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from 'react';
import {
  CustomSearchTextField,
  SimpleDropDown,
  CustomTextField,
  CustomButton,
  CustomTextButton,
  Alert,
  CustomPopover,
  DropDownItem,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { IconButton } from '@material-ui/core';
import * as fm from './formatText';
import AssetsSearcher from '../AssetsSearcher';
import * as fmGeneral from '../../GeneralFormatMessage';
import CloseIcon from '@material-ui/icons/Close';
import Collapse from '@material-ui/core/Collapse';
import SoldForm from './SoldForm';
import { Asset, AssetCategory, assetTypeCategory } from 'models/assets';
import { useUpdateEffect } from 'react-use';

const useStyles = makeStyles((theme: Theme) => ({
  businessForm: {
    maxWidth: 610,
    width: '100%',
    boxSizing: 'border-box',
    padding: 8,
    border: ' 0.5px solid rgba(0, 0, 0, 0.3)',
    boxShadow: '0px 2px 12px rgba(0, 0, 0, 0.25)',
    borderRadius: 9,
    display: 'flex',
    justifyContent: 'flex-start',
    margin: '0 auto',
    position: 'relative',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    marginBottom: 32,
    flexDirection: 'column',
    paddingTop: 30,

    [theme.breakpoints.up('sm')]: {
      paddingTop: 16,
      flexDirection: 'row',
    },
  },
  cost: {
    maxWidth: 136,
  },
  normalInput: {
    maxWidth: 172,
    marginBottom: 25,
    display: 'block',
  },
  acquiredDropdown: {
    minWidth: 74,
    width: 74,
  },
  reasonDropdown: {
    maxWidth: 172,
  },
  inputsWrap: {
    display: 'flex',
    width: '100%',
    maxWidth: 414,
    justifyContent: 'space-between',
    flexDirection: 'column',
    marginBottom: 17,

    '& > div': {
      marginBottom: 10,
    },

    [theme.breakpoints.up('sm')]: {
      width: 414,
      '& > div': {
        marginBottom: 'unset',
      },
      flexDirection: 'row',
    },
  },
  search: {
    maxWidth: 412,
    marginBottom: 25,
  },
  wrapActions: {
    width: '100%',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
  },
  remove: {
    marginTop: 28,
  },
  description: {
    fontSize: 14,
    color: 'rgba(0, 0, 0, 0.54)',
  },
  closeIcon: {
    color: theme.ptas.colors.theme.black,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
  },
  closeButton: {
    position: 'absolute',
    right: -4,
    top: -4,
  },
  alert: {
    borderRadius: '9px',
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    paddingTop: 16,
  },
  buttonManage: {
    width: 172,
    height: 38,
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '17px',
  },
  buttonManageContainer: {
    display: 'flex',
    justifyContent: 'center',
  },
}));

interface Props {
  isOpen?: boolean;
  onClose?: () => void;
  yearsAcquired: DropDownItem[];
  changeReason: DropDownItem[];
  assetsCategoryList: AssetCategory[];
  assetToUpdate?: Asset;
  onSubmit?: (asset: Asset) => Promise<void>;
  onChangeFilterParams?: (
    categoryName: string,
    categoryGroup: assetTypeCategory
  ) => void;
  allAssetsCategory: AssetCategory[];
}

export interface AssetFormData {
  id?: string | number;
  reason: string | number;
  cost: number | string;
  year: string;
  category: string;
}

function BusinessForm(props: Props): JSX.Element {
  const classes = useStyles();
  const {
    yearsAcquired,
    assetToUpdate,
    assetsCategoryList,
    changeReason,
    isOpen,
    onClose,
    onSubmit,
    onChangeFilterParams,
    allAssetsCategory,
  } = props;

  const [removeAnchor, setRemoveAnchor] = useState<HTMLElement | null>(null);
  const [openSuggestionModal, setOpenSuggestionModal] =
    useState<boolean>(false);
  const [openForm, setOpenForm] = useState<boolean>(false);
  const [asset, setAsset] = useState<Asset>(new Asset());
  const [disableButton, setDisableButton] = useState<boolean>(false);

  // validation fields
  const {
    hasError: categoryInputHasError,
    inputBlurHandler: categoryInputBlurHandler,
  } = useTextFieldValidation(
    asset.categoryCodeId ?? '',
    utilService.isNotEmpty
  );

  const { hasError: yearsInputError, inputBlurHandler: yearsInputBlurHandler } =
    useTextFieldValidation(asset.yearAcquiredId ?? '', utilService.isNotEmpty);

  const {
    hasError: reasonInputError,
    inputBlurHandler: reasonInputBlurHandler,
  } = useTextFieldValidation(
    asset.changeReason ? asset.changeReason.toString() : '',
    utilService.isNotEmpty
  );

  const { hasError: costInputError, inputBlurHandler: costInputBlurHandler } =
    useTextFieldValidation(
      asset.cost ? asset.cost.toString() : '',
      utilService.isNotEmpty
    );

  useEffect(() => {
    setOpenForm(!!isOpen);
  }, [isOpen]);

  useEffect(() => {
    setAsset(assetToUpdate ?? new Asset());

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [assetToUpdate]);

  useEffect(() => {
    const isDisable =
      !asset.yearAcquiredId || !asset.changeReason || !asset.cost;

    setDisableButton(isDisable);
  }, [asset]);

  const renderRemoveAction = (): JSX.Element => {
    const id = asset?.id ?? 0;

    return (
      <Collapse in={!!id}>
        <CustomTextButton
          ptasVariant="Danger more"
          classes={{ root: classes.remove }}
          onClick={(event: React.MouseEvent<HTMLButtonElement>): void =>
            setRemoveAnchor(event.currentTarget)
          }
        >
          {fmGeneral.remove}
        </CustomTextButton>
        <CustomPopover
          anchorEl={removeAnchor}
          onClose={closePopup}
          ptasVariant="danger"
          showCloseButton
          tail
          tailPosition="end"
          anchorOrigin={{
            vertical: 'bottom',
            horizontal: 'right',
          }}
          transformOrigin={{
            vertical: 'top',
            horizontal: 'right',
          }}
        >
          <Alert
            contentText={fm.removeAssetInfo}
            ptasVariant="danger"
            okShowButton
            okButtonText={fm.removeAsset}
            okButtonClick={handleRemove}
            classes={{
              root: classes.alert,
              buttons: classes.buttonManage,
              buttonContainer: classes.buttonManageContainer,
            }}
          />
        </CustomPopover>
      </Collapse>
    );
  };

  const handleSubmit = async (): Promise<void> => {
    onSubmit?.(asset);
    handleClose();
  };

  const handleRemove = (): void => {
    closePopup();
  };

  const closePopup = (): void => {
    setRemoveAnchor(null);
  };

  const handleClose = (): void => {
    setOpenForm(false);
    onClose?.();
    setAsset(new Asset());
  };

  const handleChangeCost = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const validNumber = isNaN(parseInt(e.target.value))
      ? 0
      : parseInt(e.target.value);

    setAsset((prev) => {
      return {
        ...prev,
        cost: validNumber,
      };
    });
  };

  const handleSelectAcquired = (item: DropDownItem): void => {
    setAsset((prev) => {
      return {
        ...prev,
        yearAcquiredId: item.value as string,
      };
    });
  };

  const handleSelectReason = (item: DropDownItem): void => {
    setAsset((prev) => {
      return {
        ...prev,
        changeReason: item.value as number,
      };
    });
  };

  const handleOpenListSuggestion = (): void =>
    setOpenSuggestionModal((prevState) => !prevState);

  useUpdateEffect(() => {
    !openSuggestionModal && categoryInputBlurHandler();
  }, [openSuggestionModal]);

  const handleSuggestionSelected = (item: AssetCategory): void => {
    handleOpenListSuggestion();

    setAsset((prev) => {
      return {
        ...prev,
        categoryCodeId: item.id,
      };
    });
  };

  const getCategoryLabel = (): string => {
    return (
      allAssetsCategory?.find((ac) => ac.id === asset.categoryCodeId)
        ?.persPropCategory ?? ''
    );
  };

  const getYearAcquireDefValue = (): string => {
    if (!yearsAcquired.length) return '';

    return yearsAcquired[0].value.toString() ?? '';
  };

  const reasonIsSold = (): boolean => {
    return (
      changeReason.find((cr) => (cr.value as number) === asset.changeReason)
        ?.label === 'Sold to Vendor'
    );
  };

  return (
    <Collapse in={openForm}>
      <div className={classes.businessForm}>
        <IconButton className={classes.closeButton} onClick={handleClose}>
          <CloseIcon className={classes.closeIcon} />
        </IconButton>
        <div>
          <CustomSearchTextField
            ptasVariant="outline"
            label={fm.refrigeratorCooler}
            classes={{ wrapper: classes.search }}
            value={getCategoryLabel()}
            error={categoryInputHasError}
            helperText={categoryInputHasError ? 'required' : ''}
            onClickInput={handleOpenListSuggestion}
            readOnly
          />
          <div className={classes.inputsWrap}>
            <SimpleDropDown
              items={yearsAcquired}
              value={asset.yearAcquiredId}
              label={fm.acquired}
              defaultValue={getYearAcquireDefValue()}
              classes={{
                textFieldRoot: classes.acquiredDropdown,
                root: classes.acquiredDropdown,
              }}
              onSelected={handleSelectAcquired}
              error={yearsInputError}
              helperText={yearsInputError ? 'required' : ''}
              onBlur={(): void => {
                yearsInputBlurHandler();
                console.log('outside');
              }}
            />
            <CustomTextField
              ptasVariant="currency"
              label={fm.cost}
              classes={{ root: classes.cost }}
              placeholder="0"
              name="cost"
              onBlur={costInputBlurHandler}
              error={costInputError}
              helperText={costInputError ? 'required' : ''}
              onChange={handleChangeCost}
              value={asset.cost}
              type="text"
            />
            <SimpleDropDown
              items={changeReason}
              label={fm.selectAReason}
              classes={{
                root: classes.normalInput,
                textFieldRoot: classes.reasonDropdown,
              }}
              onSelected={handleSelectReason}
              onBlur={reasonInputBlurHandler}
              error={reasonInputError}
              helperText={reasonInputError ? 'required' : ''}
              value={asset.changeReason}
            />
          </div>
          {/* show form if reason is sold */}
          <SoldForm isOpen={reasonIsSold()} />
          <Collapse in={!reasonIsSold()}>
            <span>{fm.includeCostForMaking}</span>
          </Collapse>
        </div>
        <div className={classes.wrapActions}>
          <CustomButton onClick={handleSubmit} disabled={disableButton}>
            {asset.id ? fmGeneral.save : fmGeneral.add}
          </CustomButton>
          {renderRemoveAction()}
        </div>
      </div>
      <AssetsSearcher
        open={openSuggestionModal}
        onClose={handleOpenListSuggestion}
        onItemSuggestionSelected={handleSuggestionSelected}
        onChangeFilterParams={onChangeFilterParams}
        assetsCategoryList={assetsCategoryList ?? []}
      />
    </Collapse>
  );
}

export default BusinessForm;
