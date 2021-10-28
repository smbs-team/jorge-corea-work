//-----------------------------------------------------------------------
// <copyright file="SeniorAppCard.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import './Home.css';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import { Link } from 'react-router-dom';
import CustomButton from '../common/CustomButton';
import * as fm from './FormatTexts';
import {
  renderIf,
  arrayNullOrEmpty,
  getDateDifferenceInDays,
} from '../../lib/helpers/util';
import { FormattedMessage } from 'react-intl';
import downArrow from '../../assets/downArrowBlack.png';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import { makeStyles, withStyles } from '@material-ui/core/styles';

const SeniorAppCard = props => {
  let yearsList = [];
  let redirected = false;
  let year = null;
  if (
    !arrayNullOrEmpty(props.seniorAppDetails) &&
    !arrayNullOrEmpty(props.appStatusCodes) &&
    !arrayNullOrEmpty(props.years)
  ) {
    year = {
      year: props.years.filter(
        y => y.yearid === props.seniorAppDetails[0].yearid
      )[0],
      status: props.appStatusCodes.filter(
        s =>
          s.state === props.seniorApp.statecode &&
          s.status === props.seniorApp.statuscode
      )[0],
      pendingDocuments:
        props.seniorAppDetails[0].missingdocumentlist &&
        props.seniorAppDetails[0].missingdocumentlist !== '',
    };
  }

  const [anchorEl, setAnchorEl] = React.useState(null);
  const [alertText, setAlertText] = React.useState('');
  const handleClick = event => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };
  const useStyles = makeStyles({
    seniorExemptionTitle: {
      fontSize: 18,

      margin: 0,
    },
  });
  let finalDate = new Date(props.seniorApp.createdon);
  finalDate.setDate(
    finalDate.getDate() + parseInt(process.env.REACT_APP_TimeOutDays)
  );
  let daysDifference = getDateDifferenceInDays(new Date(), finalDate);
  daysDifference = daysDifference < 0 ? 0 : daysDifference;

  return (
    <Card className="app-card">
      <CardContent>
        <div
          style={{
            fontSize: 18,
            fontWeight: 'bold',
            marginBottom: 8,
            lineHeight: 1.3,
          }}
          className="seniorExemptionTitle"
        >
          <FormattedMessage
            id="seniorAppCard_seniorExemption"
            defaultMessage="Senior Exemption"
          />{' '}
          {year && year.year && parseInt(year.year.name) + 1}
          <br></br>{' '}
          <FormattedMessage
            id="seniorAppCard_application"
            defaultMessage="Application"
          />
        </div>

        {renderIf(
          props.seniorApp.statuscode === 668020012,
          <div
            style={{
              marginBottom: 8,
            }}
          >
            <p
              style={{
                fontSize: 14,
                fontWeight: 'bold',
                color: '#d20000',
                lineHeight: 1.3,
              }}
            >
              <FormattedMessage
                id="home_applicationTimeOutMessage"
                defaultMessage={'{days} day{plural} to finish'}
                description="time out message on the application."
                values={{
                  days: daysDifference,
                  plural: daysDifference !== 1 ? 's' : '',
                }}
              />
            </p>
          </div>
        )}

        {renderIf(
          props.seniorApp.statuscode === 668020014,
          <Link
            to="/seniors/requestMoreInfo"
            style={{
              position: 'absolute',
              bottom: '16px',
              left: '16px',
            }}
          >
            <CustomButton
              className="display-inline"
              label={fm.moreInfoLabel}
              onClick={() => props.setSeniorApp(props.seniorApp, props.index)}
            />
          </Link>
        )}

        {renderIf(
          props.seniorApp.statuscode === 668020019,
          <div>
            <p
              style={{
                fontSize: 20,
                position: 'absolute',
                bottom: '16px',
                left: '50%',
                transform: 'translateX(-50%)',
              }}
              className="color-red"
            >
              {year && year.status ? (
                <FormattedMessage id={`${year.status.locId}`} />
              ) : (
                ''
              )}
              {/* <img src={downArrow} alt="King County" />
                  <Menu
                    id="simple-menu"
                    anchorEl={anchorEl}
                    keepMounted
                    open={Boolean(anchorEl)}
                    onClose={handleClose}
                  >
                    <MenuItem onClick={handleClose}>
                      File an appeal
                    </MenuItem>
                  </Menu> */}
            </p>
          </div>
        )}

        {renderIf(
          props.seniorApp.statuscode === 668020012,

          <Link
            to="/seniors/myInfo"
            style={{
              position: 'absolute',
              bottom: '16px',
              left: '16px',
            }}
          >
            <CustomButton
              style={{ width: '100%' }}
              className="display-inlineHome"
              label={fm.homecontinuefillingLabel}
              onClick={() => {
                props.setSeniorApp(props.seniorApp);
              }}
            />
          </Link>
        )}

        {renderIf(
          props.seniorApp.statuscode === 668020018,
          <div>
            <p
              style={{
                fontSize: 20,
                position: 'absolute',
                bottom: '16px',
                left: '50%',
                transform: 'translateX(-50%)',
              }}
              className="color-green"
            >
              {year && year.status ? (
                <FormattedMessage id={`${year.status.locId}`} />
              ) : (
                ''
              )}
            </p>
          </div>
        )}
        {renderIf(
          props.seniorApp.statuscode !== 668020012 &&
            props.seniorApp.statuscode !== 668020014 &&
            props.seniorApp.statuscode !== 668020018 &&
            props.seniorApp.statuscode !== 668020019,
          <div>
            <p
              style={{
                fontSize: 20,
                position: 'absolute',
                bottom: '16px',
                left: '50%',
                transform: 'translateX(-50%)',
              }}
              className="color-black"
            >
              {year && year.status ? (
                <FormattedMessage id={`${year.status.locId}`} />
              ) : (
                ''
              )}
            </p>
          </div>
        )}
      </CardContent>
    </Card>
  );
};

export default SeniorAppCard;
