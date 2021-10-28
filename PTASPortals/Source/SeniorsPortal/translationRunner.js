const manageTranslations = require('react-intl-translations-manager').default;

manageTranslations({
  messagesDirectory: './.messages',
  translationsDirectory: './src/translations/locales/',
  // en is defaultLocale so no need to list en here
  languages: ['en'], // any language you need
});
