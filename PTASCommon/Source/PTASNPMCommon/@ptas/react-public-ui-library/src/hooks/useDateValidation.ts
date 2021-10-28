// useDateValidation.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useState, useEffect } from 'react';

interface UseDateValidation {
  isValid: boolean;
  hasError: boolean;
  valueChangedHandler: () => void;
  inputBlurHandler: () => void;
}

const useDateValidation = (
  boundValue: string,
  validateValue: (value: string) => boolean
): UseDateValidation => {
  const [isTouched, setIsTouched] = useState(false);
  const [valueIsValid, setValueIsValid] = useState(false);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    const _isValid = validateValue?.(boundValue);
    setValueIsValid(_isValid);
    const _hasError = !_isValid && isTouched;
    setHasError(_hasError);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [boundValue /*, isTouched*/]);

  const valueChangedHandler = (): // event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  void => {
    //This is required to show validation errors while editing
    //the input value, without the input having lost the focus
    setIsTouched(true);
  };

  const inputBlurHandler = (): void => {
    setIsTouched(true);

    const _isValid = validateValue?.(boundValue);
    setValueIsValid(_isValid);
    setHasError(!_isValid);
  };

  return {
    isValid: valueIsValid,
    hasError,
    valueChangedHandler,
    inputBlurHandler,
  };
};

export default useDateValidation;
