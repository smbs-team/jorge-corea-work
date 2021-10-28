import React, { useState, Fragment } from "react";
import {
  CustomPopover as PtasCustomPopover,
  ContactInfoAlert,
  Alert,
  ContactInfoModal,
  AddressInfo,
  PhoneInfo
} from "@ptas/react-public-ui-library";
import { makeStyles } from "@material-ui/core";

const useStyles = makeStyles(() => ({
  middle: {
    position: "absolute",
    top: "300px"
  },
  bottom: {
    position: "absolute",
    bottom: "50px"
  }
}));

const CustomPopover = () => {
  const classes = useStyles();
  const [event, setEvent] = useState<HTMLElement | null>(null);
  const [contactInfoTarget, setContactInfoTarget] =
    useState<HTMLElement | null>(null);
  const [emailModalTarget, setEmailModalTarget] = useState<HTMLElement | null>(
    null
  );
  const [addressModalTarget, setAddressModalTarget] =
    useState<HTMLElement | null>(null);
  const [phoneModalTarget, setPhoneModalTarget] = useState<HTMLElement | null>(
    null
  );

  const addressList: AddressInfo[] = [
    {
      id: "1",
      title: "Auburn store",
      country: "USA",
      addressLine1: "1801 Howard Rd",
      addressLine2: "",
      city: "Auburn",
      state: "WA",
      zip: "98002"
    },
    {
      id: "2",
      title: "Headquarters",
      country: "USA",
      addressLine1: "7135 Charlotte Pike Ste 100",
      addressLine2: "",
      city: "Nashville",
      state: "TN",
      zip: "37209"
    }
  ];

  const phoneList: PhoneInfo[] = [
    {
      id: "1",
      phoneNumber: "615-799-1535",
      phoneType: "cell",
      textMessages: true
    },
    {
      id: "2",
      phoneNumber: "615-799-1577",
      phoneType: "work",
      textMessages: false
    }
  ];

  const countryItems = [
    { value: "USA", label: "USA" },
    { value: "Canada", label: "Canada" }
  ];

  const cleanState = (): void => {
    setEvent(null);
  };

  return (
    <Fragment>
      <button
        onClick={(e) => {
          setEvent(e.currentTarget);
        }}
      >
        Show popover
      </button>
      <button
        onClick={(e) => {
          setContactInfoTarget(e.currentTarget);
        }}
      >
        Show contact info alert
      </button>
      <button
        className={classes.middle}
        onClick={(e) => {
          setContactInfoTarget(e.currentTarget);
        }}
      >
        Show contact info alert (MIDDLE)
      </button>
      <button
        className={classes.bottom}
        onClick={(e) => {
          setContactInfoTarget(e.currentTarget);
        }}
      >
        Show contact info alert (BOTTOM)
      </button>
      <button
        onClick={(e) => {
          setEmailModalTarget(document.body);
        }}
      >
        Show email modal
      </button>
      <button
        onClick={(e) => {
          setAddressModalTarget(document.body);
        }}
      >
        Show address modal
      </button>
      <button
        onClick={(e) => {
          setPhoneModalTarget(document.body);
        }}
      >
        Show phone modal
      </button>
      <PtasCustomPopover
        anchorEl={event}
        onClose={(): void => {
          console.log("Closing popover");
          cleanState();
        }}
        ptasVariant='success'
        showCloseButton
        tail
        tailPosition='end'
        anchorPosition={{
          top: 0,
          left: 0
        }}
        anchorOrigin={undefined}
      >
        <Alert
          contentText='Test alert'
          ptasVariant='success'
          okShowButton
          okButtonText='Close Alert'
          okButtonClick={() => {
            cleanState();
          }}
        />
      </PtasCustomPopover>
      <ContactInfoAlert
        anchorEl={contactInfoTarget}
        onButtonClick={() => console.log("onButtonClick")}
        onClose={() => setContactInfoTarget(null)}
        items={[
          { value: "item1", label: "Item 1" },
          { value: "item2", label: "Item 2" }
        ]}
        onItemClick={(item) => console.log("Clicked item:", item)}
      />
      <ContactInfoModal
        variant={"email"}
        anchorEl={emailModalTarget}
        onClickNewItem={() => console.log("onClickNewItem")}
        onClose={() => setEmailModalTarget(null)}
        onRemoveItem={(id) => console.log("Remove email:", id)}
        emailList={["kbass@smsholdings.com", "kimberly@smsholdings.com"]}
        newItemText='New email'
        emailTexts={{
          label: "Email",
          removeButtonText: "Remove email",
          removeAlertText: "Remove email",
          removeAlertButtonText: "Remove"
        }}
      />
      <ContactInfoModal
        variant={"address"}
        anchorEl={addressModalTarget}
        onClickNewItem={() => console.log("onClickNewItem")}
        onClose={() => setAddressModalTarget(null)}
        onRemoveItem={(id) => console.log("Remove address:", id)}
        addressList={addressList}
        newItemText='New address'
        countryItems={countryItems}
        addressTexts={{
          titleLabel: "Address title",
          addressLine1Label: "Address",
          addressLine2Label: "Address (cont)",
          countryLabel: "Country",
          cityLabel: "City",
          stateLabel: "State",
          zipLabel: "Zip",
          removeButtonText: "Remove address",
          removeAlertText: "Remove address",
          removeAlertButtonText: "Remove"
        }}
      />
      <ContactInfoModal
        variant={"phone"}
        anchorEl={phoneModalTarget}
        onClickNewItem={() => console.log("onClickNewItem")}
        onClose={() => setPhoneModalTarget(null)}
        onRemoveItem={(id) => console.log("Remove phone:", id)}
        onPhoneTypeSelect={(id, phoneType) =>
          console.log("Selected phone type:", phoneType, "for id:", id)
        }
        phoneList={phoneList}
        newItemText='New phone'
        phoneTexts={{
          titleText: "Phone",
          tabCellText: "Cell",
          tabWorkText: "Work",
          tabHomeText: "Home",
          tabTollFreeText: "Toll free",
          acceptMessagesText: "Accepts text messages",
          removeButtonText: "Remove phone",
          removeAlertText: "Remove phone",
          removeAlertButtonText: "Remove"
        }}
      />
    </Fragment>
  );
};

export default CustomPopover;
