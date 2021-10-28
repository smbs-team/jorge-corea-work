module.exports = {
  root: true,
  plugins: ['@typescript-eslint', 'eslint-plugin-tsdoc', 'notice'],
  parser: '@typescript-eslint/parser',
  parserOptions: {
    project: './tsconfig.json',
    tsconfigRootDir: __dirname,
    ecmaVersion: 2018,
    sourceType: 'module',
  },
  extends: [
    'eslint:recommended',
    'plugin:@typescript-eslint/eslint-recommended',
    'plugin:@typescript-eslint/recommended',
    'react-app',
    'prettier',
  ],
  rules: {
    'tsdoc/syntax': 'warn',
    '@typescript-eslint/naming-convention': [
      'error',
      {
        selector: 'memberLike',
        modifiers: ['private'],
        format: ['camelCase'],
        leadingUnderscore: 'require',
      },
    ],
    'notice/notice': [
      'warn',
      {
        mustMatch:
          '/\\*\\*\n \\* Copyright \\(c\\) King County\\. All rights reserved\\.\n \\* @packageDocumentation\n \\*/',
        template:
          '/**\n * Copyright (c) King County. All rights reserved.\n * @packageDocumentation\n */\n\n',
        // template:
        //   "// <%= NAME %>\n/**\n * Copyright (c) King County. All rights reserved.\n * @packageDocumentation\n */\n\n",
        // varRegexps: { NAME: /(Nick|Nicholas)/ },
        messages: {
          whenFailedToMatch:
            "Couldn't find a valid copyright header, are you sure you added it?",
        },
      },
    ],
  },
};
