import React, { useState } from "react";
import {
  PropertySearcher,
  ItemSuggestion
} from "@ptas/react-public-ui-library";
import { FormattedMessage } from "react-intl";

const findMyProperty = (
  <FormattedMessage id='find_my_property' defaultMessage='Find mein property' />
);

const PropertySearcherExample = () => {
  const [list, setList] = useState<ItemSuggestion[]>([]);

  const suggestionList: ItemSuggestion[] = [
    {
      title: "Test 1",
      subtitle: "Test 1"
    },
    {
      title: "Test 2",
      subtitle: "Test 2"
    }
  ];

  const handleOnChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => {
    const inputValue = e.currentTarget.value;

    if (inputValue) {
      const newList = suggestionList.filter((item) =>
        item.title.toLowerCase().includes(inputValue.toLowerCase())
      );

      setList(newList);
    }
  };

  return (
    <div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <PropertySearcher
          label={findMyProperty}
          textButton='Locate me'
          textDescription='Enter an address, parcel #, or commercial project'
          onChange={handleOnChange}
          suggestion={{
            List: list,
            onSelected: (item) => {
              console.log(item);
            }
          }}
          onClickSearch={() => {
            console.log("searcher icon was clicked");
          }}
          onClickTextButton={() => {
            console.log("locate me button was clicked");
          }}
        />
      </div>
    </div>
  );
};

export default PropertySearcherExample;
