import { FormattedMessage } from 'react-intl';
import React from 'react';
import { arrayNullOrEmpty } from '../helpers/util';
import deepEqual from 'deep-equal';
import { removeRepeatedObjectFromArray } from './util';
import { deleteValue } from '../../services/jsonStoreService';

//INTRO: Creates messages for the dialog information on Intro
export const qualifyIntro = (dateOfBirth, householdIncome) => {
  if (
    parseFloat(householdIncome) >
    parseFloat(process.env.REACT_APP_HOUSEHOLD_INCOME_LIMIT)
  ) {
    return ['', qualifyForAllYears(false)];
  }
  if (dateOfBirth != null && getAgeBy31December(dateOfBirth) > 0) {
    return mightQualifyForTexts(dateOfBirth);
  }
  return ['', ''];
};

//MY PROPERTY: Creates messages for the dialog information on My Property info
//Takes into consideration disabled conditions and veteran disability
export const mightQualifyConsideringMyInfo = (
  dateOfBirth,
  disabled,
  spouseDisabled,
  disability
) => {
  if (disabled || spouseDisabled || disability)
    return ['', qualifyForAllYears(true)];
  else {
    return mightQualifyForTexts(dateOfBirth);
  }
};

export const returnSelectedYear = (yearsToApplyArrayObject, applicationId) => {
  if (!arrayNullOrEmpty(yearsToApplyArrayObject)) {
    return yearsToApplyArrayObject.filter(item => {
      return item.seniorAppId === applicationId && item.valid && !item.applied;
    });
  }
};

export const returnSelectedAppliedYears = (
  yearsToApplyArrayObject,
  applicationId
) => {
  return yearsToApplyArrayObject.filter(item => {
    return (
      item.seniorAppId !== applicationId &&
      item.seniorAppId !== null &&
      item.applied
    );
  });
};

export const returnSelectedCantApplyYears = yearsToApplyArrayObject => {
  return yearsToApplyArrayObject.filter(item => {
    return !item.valid;
  });
};

export const generateRadioButtomSource = (
  yearsToApplyArrayObject,
  applicationId
) => {
  let array = [];

  if (!arrayNullOrEmpty(yearsToApplyArrayObject)) {
    for (let i = 0; i <= 100; i++) {
      array.push([]);
    }

    yearsToApplyArrayObject.map((item, index) => {
      array[index][0] = item.name;
      array[index][1] = item.name;
      array[index][2] =
        item.seniorAppId === applicationId && !item.applied ? true : false;
    });

    array = array.filter(item => {
      return item.length !== 0;
    });

    let authYearsAppliedObjects = returnSelectedAppliedYears(
      yearsToApplyArrayObject,
      applicationId
    );
    let authYearsNotLongerToApplyObjects = returnSelectedCantApplyYears(
      yearsToApplyArrayObject
    );

    //It sets years that were applied.
    if (!arrayNullOrEmpty(authYearsAppliedObjects)) {
      array.map((item, index) => {
        authYearsAppliedObjects.map(authItem => {
          if (authItem.seniorAppId !== applicationId) {
            if (item[0] === authItem.name) {
              array[index][2] = false;
              array[index][3] = true;
              array[index][1] = array[index][1] + ' - already applied';
            }
          }
        });
      });
    }

    //It sets years that are not longer available to apply.
    if (!arrayNullOrEmpty(authYearsNotLongerToApplyObjects)) {
      array.map((item, index) => {
        authYearsNotLongerToApplyObjects.map(authItem => {
          if (item[1] === authItem.name) {
            array[index][1] = array[index][1] + ' - should not apply';
          }
        });
      });
    }
  }
  return array;
};

export const generateYearsObject = (yearsToApply, collectionYears) => {
  //it creates new recalculated objects that will be saved into the blob.
  let yearObject = {};
  let yearObjectArray = [];
  let selectedObject = {};

  yearsToApply.map(y => {
    selectedObject = collectionYears.filter(
      item => parseInt(item.name) === y - 1
    )[0];
    yearObject.seniorAppId = null;
    yearObject.applied = false;
    yearObject.valid = true;
    yearObject.name = parseInt(selectedObject.name) + 1;
    yearObject.yearId = selectedObject.yearid;
    yearObject.yearLocId = selectedObject.locId;
    yearObjectArray.push(yearObject);
    yearObject = {};
  });

  return yearObjectArray;
};

export const determinateYearsToApply = (
  dateOfBirth,
  disabled,
  dateOfResidence,
  yearsCollection
) => {
  //it recalculates years each time is called.
  let years = [];
  let yearsToApplyAuthObject = [];

  if (disabled) {
    years = getAllYearsArray();
  } else {
    let mightQualifyForYears = mightQualifyFor(dateOfBirth);
    years = mightQualifyForYears[0];
  }

  let yearsToApply = [];
  //validate date of acquisition
  if (dateOfResidence) {
    let d = new Date(dateOfResidence);
    let residenceYear = d.getFullYear();
    for (var i = years.length - 1; i >= 0; i--) {
      if (years[i] >= residenceYear) {
        yearsToApply.push(years[i]);
      }
    }
  } else {
    yearsToApply = years;
  }

  if (yearsToApply.length !== 0) {
    yearsToApplyAuthObject = generateYearsObject(
      yearsToApply.sort(),
      yearsCollection
    );

    // Filter current year if flagged.
    if (process.env.REACT_APP_ShowCurrentYear === 'false') {
      yearsToApplyAuthObject = yearsToApplyAuthObject.filter(
        y => y.name !== new Date().getFullYear()
      );
    }
    return yearsToApplyAuthObject;
  }

  return [];
};

//Gets the user's age by December 31 of the present year
export const getAgeBy31December = dateOfBirth => {
  let year = new Date().getFullYear();
  let december = new Date(year + '/12/31');

  let ageDifMs = december - new Date(dateOfBirth);
  let ageDate = new Date(ageDifMs); // miliseconds from epoch
  let years = Math.abs(ageDate.getUTCFullYear() - 1970);

  if (ageDate.getUTCFullYear() > 1000) {
    return years;
  }
  return 0;
};

//Determines if the user has the right age to apply in the present year
export const hasAgeToQualify = dateOfBirth => {
  if (isNaN(dateOfBirth) == false) {
    if (getAgeBy31December(dateOfBirth) > process.env.REACT_APP_AGE_LIMIT) {
      return true;
    }
  }
  return false;
};

export const mightQualifyForTexts = dateOfBirth => {
  let mightQualifyForYears = mightQualifyFor(dateOfBirth);
  let mightQualify = youMightQualifyText(mightQualifyForYears[0]);
  let mightNotQualify = youMightNotQualifyText(mightQualifyForYears[1]);
  return [mightQualify, mightNotQualify];
};

//Determines if the user can apply to a year depending on the age he
//or she has in that year
export const mightQualifyFor = dateOfBirth => {
  let ageTemp = getAgeBy31December(dateOfBirth);
  let yearTemp = new Date().getFullYear();
  let yearsMightQualify = [];
  let yearsMightNotQualify = [];

  for (var i = 0; i < 4; i++) {
    if (ageTemp > process.env.REACT_APP_AGE_LIMIT) {
      yearsMightQualify.push(yearTemp);
    } else {
      yearsMightNotQualify.push(yearTemp);
    }
    ageTemp--;
    yearTemp--;
  }

  return [yearsMightQualify, yearsMightNotQualify];
};

//Sets up the string to indicate in what years the user might qualify
export const youMightQualifyText = years => {
  let text = '';
  if (years.length == 1) {
    text = <span>{youMightQualifySingleYear(years[0])}</span>;
  } else if (years.length > 1) {
    let textTemp = '';
    for (var i = years.length - 1; i >= 1; i--) {
      textTemp = textTemp + years[i];
      if (i > 1) {
        textTemp = textTemp + ', ';
      }
    }
    text = <span>{youMightQualifyMultipleYears(textTemp, years[0])}</span>;
  }
  return text;
};

//Sets up the string to indicate in what years the user might NOT qualify
export const youMightNotQualifyText = years => {
  let text = '';
  if (years.length == 1) {
    text = <span>{youMightNotQualifySingleYear(years[0])}</span>;
  } else if (years.length > 1) {
    let textTemp = '';
    for (var i = years.length - 1; i >= 1; i--) {
      textTemp = textTemp + years[i];
      if (i > 1) {
        textTemp = textTemp + ', ';
      }
    }
    text = <span>{youMightNotQualifyMultipleYears(textTemp, years[0])}</span>;
  }
  return text;
};

export const getAllYearsArray = () => {
  let year = new Date().getFullYear();
  let years = [];
  for (var i = 0; i < 4; i++) {
    years.push(year);
    year--;
  }
  return years;
};

export const qualifyForAllYears = qualify => {
  let years = getAllYearsArray();
  if (qualify) return youMightQualifyText(years);
  else return youMightNotQualifyText(years);
};

//Texts for might qualify or might not qualify
export const youMightQualifySingleYear = year => {
  return (
    <FormattedMessage
      id="youMightQualifySingleYear"
      defaultMessage="You might qualify for {year}."
      values={{ year: year }}
    />
  );
};

export const youMightQualifyMultipleYears = (years, year) => {
  return (
    <FormattedMessage
      id="youMightQualifyMultipleYears"
      defaultMessage="You might qualify for {years} and {year} ."
      values={{ year: year, years: years }}
    />
  );
};

export const youMightNotQualifySingleYear = year => {
  return (
    <FormattedMessage
      id="youMightNotQualifySingleYear"
      defaultMessage="You might not qualify for {year}."
      values={{ year: year }}
    />
  );
};

export const youMightNotQualifyMultipleYears = (years, year) => {
  return (
    <FormattedMessage
      id="youMightNotQualifyMultipleYears"
      defaultMessage="You might not qualify for {years} or {year} ."
      values={{ year: year, years: years }}
    />
  );
};
