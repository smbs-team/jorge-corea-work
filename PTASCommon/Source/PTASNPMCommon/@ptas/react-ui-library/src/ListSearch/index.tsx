// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

 import React, { useState, Key, Fragment } from "react";
 import {
   withStyles,
   WithStyles,
   createStyles,
   TextField,
   Theme,
   TextFieldProps
 } from "@material-ui/core";
 import SearchIcon from "@material-ui/icons/Search";
 import ListItem from "./ListItem";
 import Scrollbar from "react-scrollbars-custom";
 
 /**
  * Component props
  */
 interface Props extends WithStyles<typeof useStyles> {
   data: ListItem[];
   selectedData?: number[] | string[];
   textFieldLabel?: string;
   isSelected?: boolean;
   onClick?: (selectedItem: Key, isSelected: boolean, text?: string) => void;
   TextFieldProps?: TextFieldProps;
   lockedArea?: number;
 }
 
 //TODO Miguel fix interface case, no time ATM.
 export interface ListItem {
   Key: string;
   Value: number | string;
 }
 
 /**
  * Component styles
  */
 const useStyles = (theme: Theme) =>
   createStyles({
     root: {
       width: "400px !important",
       height: "350px !important",
       padding: 16,
       paddingTop: 0,
       overflow: "auto"
     },
     textField: {
       width: "100%",
       position: "sticky",
       top: 0,
       left: theme.spacing(2),
       paddingRight: theme.spacing(4),
       marginBottom: theme.spacing(1.375),
       backgroundColor: "white"
     },
     areaList: {
       padding: theme.spacing(0, 2),
       listStyleType: "none",
       margin: 0
     },
     trackY: {
       background: "unset !important"
     }
   });
 
 /**
  * ListSearch
  *
  * @param props - Component props
  * @returns A JSX element
  */
 function ListSearch(props: Props): JSX.Element {
   const { classes, data, textFieldLabel, TextFieldProps } = props;
   const [search, setSearch] = useState<string>();
 
   return (
     <Fragment>
       <TextField
         onChange={(e): void => setSearch(e.target.value)}
         className={classes.textField}
         InputProps={{
           endAdornment: <SearchIcon />
         }}
         label={textFieldLabel ?? "Placeholder text"}
         {...TextFieldProps}
       ></TextField>
       <Scrollbar
         className={classes.root}
         trackYProps={{
           renderer: (props) => {
             const { elementRef, ...restProps } = props;
             return (
               <span
                 {...restProps}
                 ref={elementRef}
                 className={classes.trackY}
               />
             );
           }
         }}
       >
         {
           <ul className={classes.areaList}>
             {search
               ? data
                   .filter((d) =>
                     d.Key.toLowerCase().includes(search.toLowerCase())
                   )
                   .map((item, index) => (
                     <ListItem
                       key={index}
                       isSelected={
                         props.selectedData &&
                         props.selectedData.some((s: Key) => s === item.Value)
                       }
                       onClick={(isSelected): void => {
                         props.onClick &&
                           props.onClick(item.Value, isSelected, item.Key);
                       }}
                       isDeselectable={
                         props.lockedArea === (item.Value as number)
                           ? false
                           : true
                       }
                     >
                       {item.Key ?? item.Value}
                     </ListItem>
                   ))
               : data.map((item, index) => (
                   <ListItem
                     key={index}
                     isSelected={
                       props.selectedData &&
                       props.selectedData.some((s: Key) => s === item.Value)
                     }
                     onClick={(isSelected): void => {
                       props.onClick &&
                         props.onClick(item.Value, isSelected, item.Key);
                     }}
                     isDeselectable={
                       props.lockedArea === (item.Value as number) ? false : true
                     }
                   >
                     {item.Key ?? item.Value}
                   </ListItem>
                 ))}
           </ul>
         }
       </Scrollbar>
     </Fragment>
   );
 }
 
 export default withStyles(useStyles)(ListSearch);
 