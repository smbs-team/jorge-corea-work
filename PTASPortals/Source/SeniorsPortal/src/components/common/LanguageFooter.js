import React from 'react';
import SelectInput from './SelectInput';
import './LanguageFooter.css';

const LanguageFooter = props => {
  return (
    <div className="language_footer footer">
      <SelectInput
        name="languageSelection"
        id="languageSelection"
        defaultMessage="Select a language:"
        description=""
        source={props.languages}
        value={props.language}
        onChange={props.onLanguageChanged}
      />
    </div>
  );
};

export default LanguageFooter;
