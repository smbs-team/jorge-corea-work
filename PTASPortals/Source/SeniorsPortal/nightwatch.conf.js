const chrome = require('chromedriver');
const gecko = require('geckodriver');

module.exports = {
  src_folders: ['tests/e2e'],
  page_objects_path: 'tests/pages',
  custom_commands_path: 'tests/customCommands',
  output_folder: 'tests/',
  webdriver: {
    start_process: true,
  },
  test_settings: {
    default: {
      webdriver: {
        server_path: chrome.path,
        port: 9515,
        cli_args: ['--verbose'],
      },
      desiredCapabilities: {
        browserName: 'chrome',
        loggingPrefs: { driver: 'INFO', server: 'OFF', browser: 'INFO' },
        acceptInsecureCerts: true,
      },
    },
    firefox: {
      webdriver: {
        server_path: gecko.path,
        port: 4444,
        cli_args: ['--log', 'debug'],
      },
      desiredCapabilities: {
        browserName: 'firefox',
        acceptInsecureCerts: true,
      },
    },
    edge: {
      selenium: {
        start_process: true,
        server_path: 'tests/testFiles/selenium-server-standalone-3.9.1.jar',
        host: '127.0.0.1',
        port: 4444,
        cli_args: {
          'webdriver.edge.driver':
            'tests\\testFiles\\Drivers\\Edge\\v80\\msedgedriver.exe',
        },
      },
      desiredCapabilities: {
        platformName: 'Windows 10',
        browserName: 'MicrosoftEdge',
        acceptInsecureCerts: true,
        edgeOptions: {
          w3c: false,
        },
      },
    },
  },
};
