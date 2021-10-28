import React from "react";
import { CustomTabs as Tabs } from "@ptas/react-ui-library";

const CustomTabs = () => {
    return <Tabs items={["First", "Second", "Third"]} onSelected={(s) => console.log(s)}/>
}

export const CustomTabSwitch = () => {
    return <Tabs items={["First", "Second"]} onSelected={(s) => console.log(s)} switchVariant/>
}

export default CustomTabs;