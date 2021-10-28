import React from 'react';
import Grid from '@material-ui/core/Grid';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';

const MessageBox = props => {
  return (
    <div
      data-testid={props.testid}
      style={{
        margin: '3% auto',
        border: '1px solid black',
        textAlign: 'left',
        padding: '1% 1%',
        width: '80%',
        boxShadow:
          '0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19)',
      }}
    >
      <Grid
        container
        direction="row"
        justify="center"
        alignItems="center"
        xs={12}
      >
        <Grid item xs={0}>
          <div
            style={{ display: 'inline', marginRight: '10px' }}
            className="info-icon-alert"
          >
            <InfoOutlinedIcon style={{ width: '2em', height: '2em' }} />
          </div>
        </Grid>
        <Grid item xs={9}>
          <h4 style={{ fontWeight: 'bold' }}>{props.title}</h4>
        </Grid>
        <Grid item xs={2}>
          <div style={{ marginLeft: '50%' }}>
            <IconButton onClick={props.onClick}>
              <ClearIcon
                style={{
                  width: '2em',
                  height: '2em',
                  color: 'black',
                }}
              />
            </IconButton>
          </div>
        </Grid>
        <Grid item xs={12}>
          <p style={{ padding: '0 0 0 7%' }}>{props.line1}</p>
          <Grid item xs={12}>
            <p style={{ width: '68%', padding: '0 0 0 7%' }}>{props.line2}</p>
          </Grid>
        </Grid>
      </Grid>
    </div>
  );
};

export default MessageBox;
