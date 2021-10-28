import React from "react";
import { FormattedMessage, FormattedDate } from 'react-intl';

const FormatExample = () => {
  return (
      <div>
          <FormattedMessage
            id="message"
            defaultMessage="Test the Format.js library"
          />
          <br />
          <FormattedDate
            value={new Date()}
            year="numeric"
            month="long"
            day="numeric"
            weekday="long"
          />
      </div>
  );
};

export default FormatExample;
