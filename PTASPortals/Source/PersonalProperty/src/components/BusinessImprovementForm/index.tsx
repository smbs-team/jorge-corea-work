// BusinessForm.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from 'react';
import {
  SimpleDropDown,
  CustomTextField,
  CustomButton,
  CustomTextButton,
  Alert,
  CustomPopover,
  DropDownItem,
  utilService,
  useTextFieldValidation,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { IconButton } from '@material-ui/core/';
import * as fmBusinessForm from '../BusinessForm/formatText';
import * as fmGeneral from '../../GeneralFormatMessage';
import CloseIcon from '@material-ui/icons/Close';
import Collapse from '@material-ui/core/Collapse';
import SoldForm from './SoldForm';
import { Asset } from 'models/assets';

const useStyles = makeStyles((theme: Theme) => ({
  businessForm: {
    maxWidth: 788,
    width: '100%',
    boxSizing: 'border-box',
    padding: 8,
    border: ' 0.5px solid rgba(0, 0, 0, 0.3)',
    boxShadow: '0px 2px 12px rgba(0, 0, 0, 0.25)',
    borderRadius: 9,
    display: 'flex',
    justifyContent: 'flex-start',
    paddingTop: 16,
    margin: '0 auto',
    position: 'relative',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    marginBottom: 32,
    flexDirection: 'column',
    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  cost: {
    maxWidth: 120,
  },
  normalInput: {
    maxWidth: 270,
    marginBottom: 25,
    display: 'block',
  },
  acquiredDropdown: {
    minWidth: 74,
  },
  acquiredDropdownInputRoot: {
    width: 74,
  },
  typeDropdown: {
    width: 172,
  },
  reasonDropdown: {
    minWidth: 172,
  },
  inputsWrap: {
    display: 'flex',
    width: 'unset',
    maxWidth: 586,
    justifyContent: 'space-between',
    flexDirection: 'column',
    marginBottom: 17,

    '& > div': {
      marginBottom: 10,
    },

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
      width: 586,
      '& > div': {
        marginBottom: 'unset',
      },
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
  onRemove?: (id: number | string) => void;
  dataToUpdate?: Asset;
  yearsAcquired: DropDownItem[];
  changeReason: DropDownItem[];
  onSubmit?: (asset: Asset) => void;
  improvementType: DropDownItem[];
}

export interface FormData {
  id?: string | number;
  reason: string | number;
  cost: number | string;
  year: string | number;
  type: string | number;
}

function BusinessForm(props: Props): JSX.Element {
  const classes = useStyles();
  const [removeAnchor, setRemoveAnchor] = useState<HTMLElement | null>(null);
  const [openForm, setOpenForm] = useState<boolean>(false);
  const [asset, setAsset] = useState<Asset>(new Asset());
  const [disableButton, setDisableButton] = useState<boolean>(false);

  const {
    changeReason,
    improvementType,
    yearsAcquired,
    dataToUpdate,
    isOpen,
    onClose,
    onRemove,
    onSubmit,
  } = props;

  useEffect(() => {
    setOpenForm(!!isOpen);
  }, [isOpen]);

  // validation fields
  const {
    hasError: improvementTypeInputHasError,
    inputBlurHandler: improvementTypeInputBlurHandler,
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
    setAsset(dataToUpdate ?? new Asset());

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dataToUpdate]);

  const renderRemoveAction = (): JSX.Element => {
    const id = dataToUpdate?.id ?? 0;

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
            contentText={fmBusinessForm.removeAssetInfo}
            ptasVariant="danger"
            okShowButton
            okButtonText={fmBusinessForm.removeAsset}
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

  const handleSubmit = (): void => {
    onSubmit?.(asset);
    handleClose();
  };

  useEffect(() => {
    const isDisable =
      !asset.yearAcquiredId || !asset.changeReason || !asset.cost;

    setDisableButton(isDisable);
  }, [asset]);

  const handleRemove = (): void => {
    onRemove?.(asset?.id ?? -1);
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

  const handleTypeSelected = (item: DropDownItem): void => {
    setAsset((prev) => {
      return {
        ...prev,
        categoryCodeId: item.value as string,
      };
    });
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
          <div className={classes.inputsWrap}>
            <SimpleDropDown
              items={improvementType}
              error={improvementTypeInputHasError}
              helperText={improvementTypeInputHasError ? 'required' : ''}
              onBlur={improvementTypeInputBlurHandler}
              value={asset.categoryCodeId}
              label={fmBusinessForm.type}
              classes={{
                root: classes.typeDropdown,
              }}
              onSelected={handleTypeSelected}
            />
            <SimpleDropDown
              items={yearsAcquired}
              value={asset.yearAcquiredId}
              label={fmBusinessForm.acquired}
              error={yearsInputError}
              helperText={yearsInputError ? 'required' : ''}
              onBlur={yearsInputBlurHandler}
              classes={{
                textFieldRoot: classes.acquiredDropdown,
                inputRoot: classes.acquiredDropdownInputRoot,
              }}
              onSelected={handleSelectAcquired}
            />
            <CustomTextField
              ptasVariant="currency"
              label={fmBusinessForm.cost}
              classes={{ root: classes.cost }}
              placeholder="0"
              error={costInputError}
              helperText={costInputError ? 'required' : ''}
              onBlur={costInputBlurHandler}
              name="cost"
              onChange={handleChangeCost}
              value={asset.cost}
              type="text"
            />
            <SimpleDropDown
              items={changeReason}
              error={reasonInputError}
              helperText={reasonInputError ? 'required' : ''}
              onBlur={reasonInputBlurHandler}
              label={fmBusinessForm.selectAReason}
              classes={{
                root: classes.normalInput,
                textFieldRoot: classes.reasonDropdown,
              }}
              onSelected={handleSelectReason}
              value={asset.changeReason}
            />
          </div>
          {/* show form if reason is sold */}
          <SoldForm isOpen={reasonIsSold()} />
        </div>
        <div className={classes.wrapActions}>
          <CustomButton onClick={handleSubmit} disabled={disableButton}>
            {dataToUpdate ? fmGeneral.save : fmGeneral.add}
          </CustomButton>
          {renderRemoveAction()}
        </div>
      </div>
    </Collapse>
  );
}

export default BusinessForm;
