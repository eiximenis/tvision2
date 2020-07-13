class Color {

    public readonly r: number;
    public readonly g: number;
    public readonly b: number;


    constructor(r: number, g: number, b: number) {
        this.r = r;
        this.g = g;
        this.b = b;
    }   
}

interface BtermCharacter {
    codePoint: number,
    fore: Color,
    back: Color
}


export { Color, BtermCharacter };