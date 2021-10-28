/* eslint-disable */

export const data = [
  {
    ID: 1,
    Org: "WLDR",
    Alias: "Fort Pulhadburn",
    Jurisdiction: "King County",
    Description: "Proceed with caution"
  },
  {
    ID: 2,
    Org: "DDES",
    Alias: "Saint Mensomb",
    Jurisdiction: "King County",
    Description: "It's Not All It's Cracked Up To Be"
  },
  {
    ID: 3,
    Org: "WLR",
    Alias: "Lootral Du Plainsberington",
    Jurisdiction: "Seattle",
    Description: "A Dime a Dozen"
  },
  {
    ID: 4,
    Org: "GISC",
    Alias: "Blebarre Creek",
    Jurisdiction: "King County",
    Description: "When the Rubber Hits the Road"
  },
  {
    ID: 5,
    Org: "WLDR",
    Alias: "Great Santarxnard",
    Jurisdiction: "King County",
    Description: "Wouldn't Harm a Fly"
  }
];

let idCount: number = data.length + 1;

/**
 * GetItems
 *
 * @param count - Number of items to generate
 * @returns List of Items
 */
export const getItems = (count: number): any[] =>
  Array.from({ length: count }, (_v, k) => k).map(() => {
    const random: any = data[Math.floor(Math.random() * data.length)];

    const custom: any = {
      ...random,
      ID: `${idCount++}`
    };

    return custom;
  });
