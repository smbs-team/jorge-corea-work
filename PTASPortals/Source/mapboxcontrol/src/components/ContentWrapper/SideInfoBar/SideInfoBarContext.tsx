// SideInfoBarContext.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  createContext,
  PropsWithChildren,
  ComponentType,
  FC,
  useState,
  useRef,
  Dispatch,
  SetStateAction,
} from 'react';
import { ChipDataType } from '@ptas/react-ui-library';
import { BookmarkOptionType, BookmarkType } from './BookmarkItem';

interface Props {
  bookmarkItems: BookmarkType[];
  setBookmarkItems: Dispatch<SetStateAction<BookmarkType[]>>;
  bookmarkTags: ChipDataType[];
  setBookmarkTags: Dispatch<SetStateAction<ChipDataType[]>>;
  removeBookmark: (id: string) => void;
  bookmarkOptionTypes: BookmarkOptionType[];
  setBookmarkOptionTypes: Dispatch<SetStateAction<BookmarkOptionType[]>>;
  handleCreateBookmark:
    | React.MutableRefObject<((bookmark: BookmarkType) => void) | undefined>
    | undefined;
  handleEditBookmark:
    | React.MutableRefObject<((bookmark: BookmarkType) => void) | undefined>
    | undefined;
  handleRemoveBookmark:
    | React.MutableRefObject<((bookmarkId: string) => void) | undefined>
    | undefined;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const SideInfoBarContext = createContext<Props>(null as any);

export const withSideInfoBar = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  const [bookmarkItems, setBookmarkItems] = useState<BookmarkType[]>([]);
  const [bookmarkTags, setBookmarkTags] = useState<ChipDataType[]>([]);
  const [bookmarkOptionTypes, setBookmarkOptionTypes] = useState<
    BookmarkOptionType[]
  >([]);
  const handleCreateBookmark = useRef<(bookmark: BookmarkType) => void>();
  const handleEditBookmark = useRef<(bookmark: BookmarkType) => void>();
  const handleRemoveBookmark = useRef<(bookmarkId: string) => void>();

  const removeBookmark = (id: string): void => {
    handleRemoveBookmark?.current?.(id);
  };
  return (
    <SideInfoBarContext.Provider
      value={{
        bookmarkItems,
        bookmarkOptionTypes,
        bookmarkTags,
        handleCreateBookmark,
        handleEditBookmark,
        handleRemoveBookmark,
        removeBookmark,
        setBookmarkItems,
        setBookmarkOptionTypes,
        setBookmarkTags,
      }}
    >
      <Component {...props}>{props.children}</Component>
    </SideInfoBarContext.Provider>
  );
};
