import { WithStyles, Theme } from "@material-ui/core";

/**
 * Makes classes optional
 *
 * @remarks
 * See: https://github.com/mui-org/material-ui/issues/14544
 */
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export type GenericWithStyles<T extends WithStyles<any, any>> = Omit<
  T,
  "theme" | "classes"
> & { classes?: Partial<T["classes"]> } & ("theme" extends keyof T
    ? { theme?: Theme }
    : {});

/**
 * Pick required keys
 */
export type PickRequiredKeys<T> = {
  [K in keyof T]-?: {} extends Pick<T, K> ? never : K;
}[keyof T];

/**
 * Pick optional keys
 */
export type PickOptionalKeys<T> = {
  [K in keyof T]-?: {} extends Pick<T, K> ? K : never;
}[keyof T];

/**
 * Make specified keys optional
 */
export type Optional<T, U extends keyof T> = Omit<T, U> & Partial<Pick<T, U>>;
