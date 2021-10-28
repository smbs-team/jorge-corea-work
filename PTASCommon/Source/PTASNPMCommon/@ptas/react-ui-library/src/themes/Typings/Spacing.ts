interface Widths {
  maximum: number;
  minimum: number;
}

export interface Spacing {
  widths: {
    content: Widths;
    form: Widths;
    window: Widths;
  };
}
