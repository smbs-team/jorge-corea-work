import React from 'react';
import Grid from '@material-ui/core/Grid';
import NumberFormat from 'react-number-format';
import { FormattedMessage } from 'react-intl';
import { number } from 'prop-types';

const TotalIncome = props => {
  return (
    <div className="house-hold-income">
      <Grid>
        {/*<FormattedMessage
          id="totalHouseIncomeT"
          defaultMessage={
            props.globalIncome
              ? 'Total household income is $ {totalIncome}'
              : 'Total income is $ {totalIncome}'
          }
          values={{
            totalIncome: props.totalFormsIncome,
          }}
        />*/}
        <FormattedMessage
          id="totalHouseIncomeT"
          defaultMessage="Total household income is ${totalIncome}"
          values={{
            totalIncome: (
              <NumberFormat
                value={props.totalFormsIncome}
                displayType={'text'}
                thousandSeparator={true}
              />
            ),
          }}
        />
      </Grid>
    </div>
  );
};

export default TotalIncome;
