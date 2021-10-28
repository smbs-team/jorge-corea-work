interface Widths {
    maximum: number;
    minimum: number;
}

export interface Spacing {
    widths: {
        content: Widths,
        form: Widths,
        window: Widths
    };
    between: {
        b1: number;
        b2: number;
        b3: number;
        b4: number;
        b5: number;
        b6: number;
        b7: number;
    }
}