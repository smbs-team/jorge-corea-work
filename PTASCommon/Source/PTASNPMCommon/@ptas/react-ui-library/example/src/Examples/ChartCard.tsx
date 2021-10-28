import React from "react";
import { ChartCard as Card } from "@ptas/react-ui-library";

const ChartCard = () => {
  return (
    <Card
      imgSrc='https://placeimg.com/640/480/animals'
      onMenuOptionClick={(z: any) => console.log(z)}
      author='Todd McMeeKin'
      title='Descriptive statistics'
      date='Wed., June 3, 2020  12:43pm'
      menuItems={[
        { id: 0, label: "Uno" },
        { id: 1, label: "Dos" }
      ]}
      showLock
      onLockClick={(status) => console.log("is locked?", status)}
      isLocked
    />
  );
};

export default ChartCard;
