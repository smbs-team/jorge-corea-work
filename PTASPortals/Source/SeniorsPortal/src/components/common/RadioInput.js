/*

 
   
  }
}
*/
import React from 'react';

import { makeStyles } from '@material-ui/core/styles';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormHelperText from '@material-ui/core/FormHelperText';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import { withStyles } from '@material-ui/core/styles';

import InputLabel from '@material-ui/core/InputLabel';
import Input from '@material-ui/core/Input';
import MenuItem from '@material-ui/core/MenuItem';
import Information from '../common/Information';
import { renderIf, arrayNullOrEmpty } from '../../lib/helpers/util';
import PropTypes from 'prop-types';

import { v4 as uuidV4 } from 'uuid';

const MenuItemCSS = withStyles({
  selected: {
    backgroundColor: '#a5c727 !important',
  },
})(MenuItem);

const FormControlCSS = withStyles({
  root: {
    '& label.Mui-focused': {
      color: '#a5c727',
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: '#a5c727',
    },
    '& .MuiInput-root': {
      width: '180px',
    },
    '& .MuiFormLabel-root': {
      width: '320px',
    },
  },
})(FormControl);

const styles = theme => ({
  select: {},
});

const useStyles = makeStyles(theme => ({
  formControl: {
    margin: theme.spacing(3),
  },
}));

class RadioInput extends React.Component {
  constructor(props) {
    super(props);
  }

  InformationAvailable() {
    if (this.props.information != null) {
      return (
        <Information
          style={{
            verticalAlign: 'bottom',
            marginLeft: '105px',
            marginTop: '10px',
          }}
        />
      );
    }
  }

  render() {
    const { classes, itemMsg } = this.props;
    let input = this.props.readOnly ? (
      <Input id="select1" readOnly />
    ) : (
      <Input id="select1" />
    );

    return (
      <div className="align-left">
        <div style={{ display: 'inline-block' }}>
          <FormControl component="fieldset" className={classes.formControl}>
            <RadioGroup
              name={this.props.name}
              displayEmpty={this.props.displayEmpty}
              value={this.props.value}
              onChange={this.props.onChange}
              input={input}
              InputLabelProps={{
                //Show label and placeholder at the same time
                shrink: true,
              }}
              className={classes.select}
            >
              {renderIf(
                this.props.sourceLabel,
                !arrayNullOrEmpty(this.props.source)
                  ? this.props.source.map((option, index) => (
                      <React.Fragment>
                        <FormControlLabel
                          //disabled={option[3] !== null ? option[3] : true}
                          key={uuidV4()}
                          data-testid={'radio' + index}
                          disabled={option[3]}
                          checked={option[2]}
                          value={option[this.props.sourceValue]}
                          control={<Radio color="#a5c727" />}
                          label={option[this.props.sourceLabel]}
                          labelPlacement="start"
                        />
                        {option[2] && itemMsg && (
                          <span>
                            {itemMsg[option[this.props.sourceLabel]] || ''}
                          </span>
                        )}
                      </React.Fragment>
                    ))
                  : '',
                !arrayNullOrEmpty(this.props.source)
                  ? this.props.source.map((option, index) => (
                      <React.Fragment>
                        <FormControlLabel
                          key={uuidV4()}
                          data-testid={'radio' + index}
                          //disabled={option[3] !== null ? option[3] : true}
                          disabled={option[3]}
                          checked={option[2]}
                          value={option[0]}
                          control={<Radio color="#a5c727" />}
                          label={option[1]}
                          labelPlacement="end"
                        />
                        {option[2] && itemMsg && (
                          <span>{itemMsg[option[1]] || ''}</span>
                        )}
                      </React.Fragment>
                    ))
                  : ''
              )}
            </RadioGroup>
            <FormHelperText></FormHelperText>
          </FormControl>
        </div>
        {this.InformationAvailable()}
      </div>
    );
  }
}

RadioInput.propTypes = {
  classes: PropTypes.object.isRequired,
  itemMsg: PropTypes.string,
};

export default withStyles(styles)(RadioInput);
