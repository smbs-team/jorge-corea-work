type Variant =
  | 'h1'
  | 'h2'
  | 'h3'
  | 'h4'
  | 'h5'
  | 'h6'
  | 'buttonLarge'
  | 'buttonSmall'
  | 'formLabel'
  | 'formField'
  | 'body'
  | 'bodyBold'
  | 'bodyExtraBold'
  | 'bodyLarge'
  | 'bodyLargeBold'
  | 'bodySmall'
  | 'bodySmallBold'
  | 'finePrint'
  | 'finePrintBold'

interface TypographyStyles {
    fontSize: string,
    fontWeight: number,
}

export interface Typography extends Record<Variant, TypographyStyles> {
    lineHeight: number;
    bodyFontFamily: string;
    titleFontFamily: string;
}