export const getUserType = user => {
  return user
    ? user.isTaxpayer
      ? 'Taxpayer'
      : user.isSpouse
      ? 'Spouse'
      : user.isOccupant
      ? 'Occupant'
      : ''
    : '';
};
