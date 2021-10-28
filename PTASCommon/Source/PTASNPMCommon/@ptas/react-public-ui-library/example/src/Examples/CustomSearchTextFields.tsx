import React, { useState } from "react";
import {
  CustomSearchTextField,
  ItemSuggestion
} from "@ptas/react-public-ui-library";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(() => ({
  narrow: {
    width: "45px"
  },
  shrink: {
    left: -6
  }
}));

const CustomButtonComp = () => {
  const classes = useStyles();
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
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomSearchTextField
          ptasVariant='underline'
          label='Underline'
          onChange={handleOnChange}
          suggestion={{
            List: list,
            onSelected: (item) => {
              console.log(item);
            }
          }}
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomSearchTextField
          ptasVariant='underline tall'
          label='Underline tall'
          onClick={() => {
            console.log("Was clicked");
          }}
          suggestion={{
            List: [],
            loading: true
          }}
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomSearchTextField
          ptasVariant='squared outline'
          label='Squared Outline'
          onChange={handleOnChange}
          onChangeDelay={500}
          suggestion={{
            List: list,
            onSelected: (item) => {
              console.log(item);
            }
          }}
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomSearchTextField
          classes={{ wrapper: classes.narrow, shrinkRoot: classes.shrink }}
          ptasVariant='squared outline'
          label='State'
          hideSearchIcon
        />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomSearchTextField ptasVariant='outline' label='Outline' />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <CustomSearchTextField
          ptasVariant='outline tall'
          label='Outline tall'
        />
      </div>
      <div
        style={{
          marginRight: 5,
          marginLeft: 5,
          padding: 10,
          background: "black"
        }}
      >
        <CustomSearchTextField
          ptasVariant='inverse'
          label='Outline tall'
          suggestion={{
            List: [],
            onSelected: (item) => {
              console.log(item);
            }
          }}
        />
      </div>
    </div>
  );
};

export default CustomButtonComp;
