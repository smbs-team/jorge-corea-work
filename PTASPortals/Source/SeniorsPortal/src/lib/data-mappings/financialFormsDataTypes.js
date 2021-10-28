import { convertToMoneyFormat } from '../helpers/util';

export const transformToType = (input, type) => {
  if (input !== undefined && input !== null && input !== NaN) {
    switch (type) {
      case 'currency':
        return '$ ' + input;
      case 'bool':
        return input == '1' ? 'YES' : 'NO';
      default:
        return input;
    }
  }

  return input;
};
