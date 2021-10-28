import React, { Component } from 'react';
import './Login.css';
import { AuthConsumer } from '../contexts/AuthContext';
import Button from '@material-ui/core/Button';
import { FormattedMessage } from 'react-intl';

class Login extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render = () => {
    return (
      <AuthConsumer>
        {({ login }) => (
          <div className="login">
            <div>
              <FormattedMessage
                id="login_message"
                defaultMessage="Please Log in by clicking the button below."
                description="login message"
              />
            </div>
            <Button variant="contained" onClick={login}>
              <FormattedMessage
                id="login"
                defaultMessage="Sign in"
                description="login button text"
              />
            </Button>
          </div>
        )}
      </AuthConsumer>
    );
  };
}

export default Login;
