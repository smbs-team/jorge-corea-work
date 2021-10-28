import React, { useState, Fragment } from "react";
import {
  CustomPopover as PtasCustomPopover,
  Save
} from "@ptas/react-ui-library";

const CustomPopover = () => {
  const [event, setEvent] = useState<HTMLElement | null>(null);
  const cleanState = (): void => {
    setEvent(null);
  };

  const folders = [
    {
      title: "title",
      subject: "subject",
      id: "1234",
      parent: null
    }
  ];

  return (
    <Fragment>
      <button
        onClick={(e) => {
          setEvent(e.currentTarget);
        }}
      >
        Show popover
      </button>
      <PtasCustomPopover
        anchorEl={event}
        onClose={(): void => {
          console.log("Closing popover");
          cleanState();
        }}
      >
        <Save
          title='Save'
          dropdownRows={folders}
          newFolderDropdownRows={folders}
          okClick={(): void => console.log("Ok")}
          newFolderOkClick={(): void => console.log("New ok")}
        />
      </PtasCustomPopover>
    </Fragment>
  );
};

export default CustomPopover;
