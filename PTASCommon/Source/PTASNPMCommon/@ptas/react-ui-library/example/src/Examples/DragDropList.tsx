import React, { Fragment } from 'react';
import { DragDropList as DragDrop, DragDropItem } from "@ptas/react-ui-library";

class DummyClass {
    id: string;
    name: string;
    constructor(fields: DummyClass){
        this.id = fields.id;
        this.name = fields.name;
    }
}

const DragDropList = () => {
    // const items: DummyClass[] = [{id: '1', name: 'item 1'}, {id: '2', name: 'item 2'}];
    // const initialData: DragDropItem<DummyClass>[] = items.map((d) => ({
    //     id: d.id,
    //     content: d.name,
    //     originalRow: d
    // }));
    // return (
    //     <Fragment>
    //         <DragDrop<DummyClass> initial={initialData} />
    //     </Fragment>
    // )
}

export default DragDropList;