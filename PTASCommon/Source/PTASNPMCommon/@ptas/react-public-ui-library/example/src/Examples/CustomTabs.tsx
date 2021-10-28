import React, { useState, useRef, useEffect } from "react";
import { CustomPopover, CustomTabs } from "@ptas/react-public-ui-library";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(() => ({
  itemCustomColor: {
    color: "lightblue"
  }
}));

const CustomTabsComp = () => {
  const classes = useStyles();
  const [popoverAnchor, setPopoverAnchor] = useState<HTMLElement | null>(null);
  const tabRef = useRef(null);

  useEffect(() => {
    console.log("ref in custom tab", tabRef);
  }, [tabRef]);

  return (
    <div style={{ display: "flex", flexDirection: "column" }}>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Simple</span>
        <CustomTabs
          opacityColor='#fff'
          ptasVariant='Simple'
          items={[
            { label: "Item 1", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 3", disabled: true },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
          }}
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          CustomLabels
        </span>
        <CustomTabs
          ptasVariant='CustomLabels'
          defaultSelectedIndex={-1}
          opacityColor='#fff'
          items={[
            {
              label: (
                <label style={{ color: "red", cursor: "pointer" }}>
                  Item 1
                  <ExpandMoreIcon
                    style={{ width: 14, verticalAlign: "middle" }}
                  />
                </label>
              ),
              disabled: false
            },
            { label: "Item 2", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false },
            { label: "Item 3", disabled: false }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
          }}
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>Switch</span>
        <CustomTabs
          ptasVariant='Switch'
          items={[
            { label: "Item 1", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 3 largo", disabled: false }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
          }}
          defaultSelectedIndex={2}
          tabsBackgroundColor='#187089'
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          SwitchMedium
        </span>
        <CustomTabs
          ptasVariant='SwitchMedium'
          items={[
            { label: "Item 1", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 3", disabled: false }
          ]}
          tabsBackgroundColor='#7EA000'
          itemTextColor='white'
          selectedItemTextColor='black'
          indicatorBackgroundColor='white'
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
          }}
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          SwitchSmall
        </span>
        <CustomTabs
          ptasVariant='SwitchSmall'
          items={[
            { label: "Item 1", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 3", disabled: false }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
          }}
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          Switch - Disabled
        </span>
        <CustomTabs
          ptasVariant='Switch'
          items={[
            { label: "Item 1", disabled: true },
            { label: "Item 2", disabled: true },
            { label: "Item 3", disabled: true }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
          }}
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "5px" }}>
          Simple - Custom colors
        </span>
        <CustomTabs
          classes={{ itemSimple: classes.itemCustomColor }}
          ptasVariant='Simple'
          items={[
            { label: "Item 1", disabled: false },
            { label: "Item 2", disabled: false },
            { label: "Item 3", disabled: false, targetRef: tabRef }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
            if (tab === 2) {
              setPopoverAnchor(tabRef.current);
            }
          }}
          indicatorBackgroundColor={"green"}
          itemTextColor={"red"}
        />
      </div>
      <CustomPopover
        anchorEl={popoverAnchor}
        onClose={(): void => {
          setPopoverAnchor(null);
        }}
      >
        <div>Test popover</div>
      </CustomPopover>
    </div>
  );
};

export default CustomTabsComp;
