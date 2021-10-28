//-----------------------------------------------------------------------
// <copyright file="App.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import 'core-js';
import React from 'react';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import SeniorsContainer from './containers/SeniorsContainer';
import Intro from './components/intro/Intro';
import Home from './components/home/Home';
import CropperComponent from './components/common/cropper-component/cropperComponent';
import './App.css';
import AuthRoute from './routes/AuthRoute';
import { MuiThemeProvider } from '@material-ui/core/styles';
import mainTheme from './themes/mainTheme';

import { IntlProvider } from 'react-intl';
import LanguageFooter from './components/common/LanguageFooter';
import translations from './translations/locales';
import { renderIf } from './lib/helpers/util';
import Header from './components/common/Header';
import Footer from './components/common/Footer';
import { CollectionProvider } from './contexts/CollectionContext';
import { SignalRProvider } from './contexts/SignalRContext';
import ProtectedRoute from './routes/ProtectedRoute';
import SignIn from './components/sign-in/SignIn';

/* Initial class for the application, keeps track of the language, the different context and the different routes for the application.
 *
 * @class App
 * @extends {React.Component}
 */
class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      language: 'en',
      //language: navigator.language.split(/[-_]/)[0],
      languages: [['en', 'English (US)']],
    };
  }

  onLanguageChanged = e => {
    this.setState({ language: e.target.value });
  };

  //const language = navigator.language.split(/[-_]/)[0];

  render() {
    return (
      <IntlProvider
        locale={this.state.language}
        defaultLocale={this.state.language}
        key={this.state.language}
        messages={translations[this.state.language]}
      >
        <MuiThemeProvider theme={mainTheme}>
          <div className="App">
            {renderIf(
              process.env.REACT_APP_SHOW_LANGUAGE === true,
              <LanguageFooter
                languages={this.state.languages}
                language={this.state.language}
                onLanguageChanged={this.onLanguageChanged}
              />
            )}

            <BrowserRouter>
              <AuthProvider>
                <SignalRProvider>
                  <CollectionProvider>
                    <Header className="navbar" />
                    <div className="main-container">
                      <Switch>
                        <Route path="/intro" component={Intro} />
                        <Route path="/signin" component={SignIn} />

                        <ProtectedRoute path="/home" component={Home} />
                        <ProtectedRoute path="/home/:id/" component={Home} />
                        <ProtectedRoute
                          path="/cropper"
                          component={CropperComponent}
                        />
                        <ProtectedRoute
                          path="/seniors"
                          component={SeniorsContainer}
                        />
                        <ProtectedRoute
                          path="/seniors/:id"
                          component={SeniorsContainer}
                        />
                        <AuthRoute path="/" component={Home} />
                      </Switch>
                    </div>

                    <Footer className="footer" />
                  </CollectionProvider>
                </SignalRProvider>
              </AuthProvider>
            </BrowserRouter>
          </div>
        </MuiThemeProvider>
      </IntlProvider>
    );
  }
}

export default App;
