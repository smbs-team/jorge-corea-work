import React from "react";
import { IntlProvider } from "react-intl";
import translations from "./translations/locales";
import RedactionTool from "./RedactionTool";

/**
 * This is the main App component. Will continue base configuration elements.
 *
 * @class App
 * @extends {React.Component}
 */
class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render() {
    return (
      <IntlProvider
        locale={"en"}
        defaultLocale={"en"}
        key={"en"}
        messages={translations["en"]}
      >
        <RedactionTool />
      </IntlProvider>
    );
  }
}

export default App;
