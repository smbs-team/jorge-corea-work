import React, { useMemo } from "react";
import {
  SideInfoBar,
  SideInfoBarParcelDetails,
  MenuOption
} from "@ptas/react-ui-library";
import errorImage from "./no-image.png";

const SideInfo = () => {
  const parcelDetails: SideInfoBarParcelDetails[] = [
    { name: "Buildings", value: "1" },
    { name: "Built", value: "1953" },
    { name: "Renovate", value: "No" },
    { name: "Stories", value: "1.5" },
    { name: "Units", value: "1" },
    { name: "Grade", value: "7" },
    { name: "predominantUse", value: "Single Family" }
  ];

  const images = [
    <img key='1' alt='' src='https://picsum.photos/id/237/200' />,
    <img key='2' alt='' src='https://picsum.photos/id/1025/200' />,
    <img key='3' alt='' src='https://picsum.photos/id/1047/200' />
  ];

  const parcelInfoViewMenuItems: MenuOption[] = useMemo(
    () => [
      {
        id: "parcelDetails",
        label: "Parcel details"
      },
      {
        id: "streetView",
        label: "Street View"
      },
      {
        id: "obliques",
        label: "Obliques"
      },
      {
        id: "bookmarks",
        label: "Bookmarks"
      }
    ],
    []
  );

  return (
    <SideInfoBar
      parcelNumber='778740-0055'
      parcelDetails={parcelDetails}
      onClose={() => console.log("Hide me!")}
      images={[...images, <img src={errorImage}></img>]}
      numberOfBookmarks={21}
      checkListName='Bookmarks'
      viewMenuItems={parcelInfoViewMenuItems}
      onChecklistButtonClick={() => console.log("checklist clicked!")}
      onViewOptionClick={(action) => {}}
    />
  );
};

export default SideInfo;
