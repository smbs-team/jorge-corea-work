// mapCollection.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

interface SetMapCollectionParams<I, V> {
  map: Map<I, V>;
  id: I;
  value: V | ((prev: V) => V);
  setStateFn?: React.Dispatch<React.SetStateAction<Map<I, V>>>;
}

interface RemoveMapCollectionParams<I, V> {
  map: Map<I, V>;
  id: I;
  setStateFn?: React.Dispatch<React.SetStateAction<Map<I, V>>>;
}

/**
 * set a item to map collection
 */
export function setMapCollection<I, V>(
  params: SetMapCollectionParams<I, V>
): Map<I, V> {
  const { map, id, value, setStateFn } = params;
  const setValueFn = value as (prev: V) => V;
  const newMapEntity = new Map(map);
  const itemFound = newMapEntity.get(id);
  const newValue =
    typeof setValueFn === 'function'
      ? setValueFn(itemFound as V)
      : (value as V);
  newMapEntity.set(id, newValue);
  // if setState function is passed
  setStateFn && setStateFn(newMapEntity);
  return newMapEntity;
}

/**
 * remove item from map collection
 */
export function removeItemFromMapCollection<I, V>(
  params: RemoveMapCollectionParams<I, V>
): Map<I, V> {
  const { map, id, setStateFn } = params;
  const newMapEntity = new Map(map);
  newMapEntity.delete(id);
  // if setState function is passed
  setStateFn && setStateFn(newMapEntity);
  return newMapEntity;
}
