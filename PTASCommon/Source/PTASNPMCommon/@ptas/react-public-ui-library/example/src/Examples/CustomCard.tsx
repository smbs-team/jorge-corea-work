import React, { useRef } from "react";
import {
  CustomCard,
  CardWithSlider,
  CardPropertyInfo,
  CardWithActions,
  Alert,
  CustomPopover,
  CardPersonalProperty
} from "@ptas/react-public-ui-library";
import { Box, makeStyles } from "@material-ui/core";
import { useState } from "react";

const useStyles = makeStyles(() => ({
  simpleRoot: {
    backgroundColor: "lightblue",
    width: 200,
    alignItems: "center"
  },
  simpleContent: {
    paddingBottom: "48px !important"
  },
  cardWithSliderRoot: {
    // width: 300
  },
  cardWithSliderContent: {},
  rootWrap: {
    width: "100%",
    maxWidth: 646
  },
  /**
   * CardPropertyInfo
   */
  propertyInfoRoot: {
    width: 700
  }
}));

const CustomCardComp = () => {
  const classes = useStyles();
  const [sliderValue, setSliderValue] = useState<number>(0);
  const [
    propertyInfoTabsValue,
    setPropertyInfoTabsValueSliderValue
  ] = useState<number>(-1);
  const tabRef = useRef(null);
  const [popoverAnchor, setPopoverAnchor] = useState<HTMLElement | null>(null);

  return (
    <div style={{ display: "flex", flexDirection: "column" }}>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>
          Card wrapper
        </span>
        <CustomCard
          shadow
          classes={{
            rootWrap: classes.rootWrap
          }}
          variant='wrapper'
        >
          <div>Card wrapper</div>
        </CustomCard>
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>Simple</span>
        <CustomCard
          shadow
          classes={{ root: classes.simpleRoot, content: classes.simpleContent }}
        >
          Hello Card
        </CustomCard>
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>
          Card with slider
        </span>
        <CardWithSlider
          selectedIndex={0}
          shadow
          title={"Auburn Center Dry Cleaning"}
          subtitle={"Parcel #801860-0533"}
          options={[
            { label: "None", disabled: false },
            { label: "View", disabled: false },
            { label: "Edit", disabled: false }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
            setSliderValue(tab);
          }}
          tabsBackgroundColor={sliderValue === 0 ? "#666666" : "#7EA000"}
          itemTextColor={"#ffffff"}
          selectedItemTextColor={"#000000"}
          indicatorBackgroundColor={"#ffffff"}
          classes={{
            root: classes.cardWithSliderRoot,
            cardContent: classes.cardWithSliderContent
          }}
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>
          Card for business personal property
        </span>
        <CardPersonalProperty
          type={"business"}
          filedDate={Date.now().toString()}
          assessedYear={'2021'}
          shadow
          title={"Auburn Center Dry Cleaning"}
          subtitle={"Personal property account #22822001"}
          onOutlineButtonClick={(evt) =>
            console.log("Click outline button:", evt)
          }
          onTextButtonClick={(evt) => console.log("Click text button:", evt)}
          outlineButtonText='Manage'
          textButtonText='Who can manage'
        />
        <br />
        <CardPersonalProperty
          type={"person"}
          shadow
          title={"David Wendingham"}
          subtitle={"Last seen 3/3/2020"}
          onOutlineButtonClick={(evt) =>
            console.log("Click outline button:", evt)
          }
          onTextButtonClick={(evt) => console.log("Click text button:", evt)}
          outlineButtonText='Edit'
          textButtonText='Has access to'
        />
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>
          Card for property info
        </span>
        <CardPropertyInfo
          id={"1234"}
          shadow={false}
          classes={{ root: classes.propertyInfoRoot }}
          type={"full"}
          imageSrc={
            "https://images.adsttc.com/media/images/5ecd/d4ac/b357/65c6/7300/009d/large_jpg/02C.jpg?1590547607"
          }
          parcelNumber={"801860-0533"}
          propertyName={"WILLIAMS-HART CAMERON+ANDRIA"}
          propertyAddress={"12403 14th Ave S, Burien, WA 98168"}
          mailName={"WILLIAMS-HART CAMERON+ANDRIA"}
          mailAddress={"12403 14th Ave S, Burien, WA 98168"}
          options={[
            { label: "Update mailing address", disabled: false },
            { label: "Go paperless", disabled: false },
            { label: "More", disabled: false },
            { label: "Remove", disabled: false, targetRef: tabRef }
          ]}
          onSelected={(tab: number) => {
            if (tab === 3) {
              //Remove button
              setPopoverAnchor(tabRef.current);
              console.log("Remove action");
            } else {
              setPropertyInfoTabsValueSliderValue(tab);
            }
          }}
          tabsBackgroundColor={"transparent"}
          itemTextColor={"#187089"}
          selectedItemTextColor={"#187089"}
          indicatorBackgroundColor={"#000000"}
        >
          {propertyInfoTabsValue >= 0 && (
            <Box>Children {propertyInfoTabsValue}</Box>
          )}
        </CardPropertyInfo>
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>
          Card for property info
        </span>
        <CardPropertyInfo
          id={"4321"}
          shadow={false}
          type={"simple"}
          imageSrc={
            "https://images.adsttc.com/media/images/5ecd/d4ac/b357/65c6/7300/009d/large_jpg/02C.jpg?1590547607"
          }
          parcelNumber={"801860-0533"}
          propertyName={"WILLIAMS-HART CAMERON+ANDRIA"}
          propertyAddress={"12403 14th Ave S, Burien, WA 98168"}
          mailName={"WILLIAMS-HART CAMERON+ANDRIA"}
          mailAddress={"12403 14th Ave S, Burien, WA 98168"}
          upperStripText={"Applied 2/16/2021"}
          options={[
            { label: "Update mailing address", disabled: false },
            { label: "Go paperless", disabled: false },
            { label: "More", disabled: false },
            { label: "Remove", disabled: false, targetRef: tabRef }
          ]}
          onSelected={(tab: number) => {
            console.log("Tab selected:", tab);
          }}
          tabsBackgroundColor={"transparent"}
          itemTextColor={"#187089"}
          selectedItemTextColor={"#187089"}
          indicatorBackgroundColor={"#000000"}
        >
          <Box>Children</Box>
        </CardPropertyInfo>
      </div>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>
          Card with actions
        </span>
        <div style={{ display: "flex" }}>
          <CardWithActions
            shadow={false}
            title={"Senior Exemption"}
            text={"Based on your ownership, occupancy, age, and income"}
            actions={[
              { text: "Apply", type: "button", buttonStyle: "default" }
            ]}
            onActionClick={(action) => {
              console.log("Clicked action:", action);
            }}
          />
          &nbsp;&nbsp;&nbsp;&nbsp;
          <CardWithActions
            shadow={false}
            title={"Senior Exemption"}
            text={"Based on your ownership, occupancy, age, and income"}
            actions={[
              { text: "Applied", type: "label", labelStyle: "success" },
              {
                text: "More info requested",
                type: "label",
                labelStyle: "warning"
              },
              { text: "View", type: "button", buttonStyle: "text" }
            ]}
            onActionClick={(action) => {
              console.log("Clicked action:", action);
            }}
          />
        </div>
      </div>
      <CustomPopover
        anchorEl={popoverAnchor}
        onClose={(): void => {
          setPopoverAnchor(null);
        }}
        ptasVariant='danger'
        showCloseButton
        tail
        tailPosition='end'
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "right"
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "right"
        }}
      >
        <Alert
          contentText='Test alert'
          ptasVariant='danger'
          okButtonText='Close Alert'
          okButtonClick={(): void => {
            setPopoverAnchor(null);
          }}
        />
      </CustomPopover>
    </div>
  );
};

export default CustomCardComp;
