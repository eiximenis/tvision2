import {BtermCharacter, Color} from './bterm-character.js'


const _terminals = {};

function Attach(id: string): Bterm | null {
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (canvas) {
        const terminal = new Bterm(canvas);
        console.log('Added bterm with id ' + id);
        _terminals[id] = terminal;
        return terminal;
    }
    else {
        return null;
    }
}

function GetBterm(id: string): Bterm | null {
    if (_terminals[id]) return _terminals[id] as Bterm;
    return null;
}


class Bterm {
    private _canvas: HTMLCanvasElement;

    private _cols: number = 80;
    private _rows: number = 24;

    constructor(canvas: HTMLCanvasElement) {
        this._canvas = canvas
        const ctx = this._canvas.getContext('2d') as CanvasRenderingContext2D;
        ctx.fillStyle = "black";
        ctx.fillRect(0, 0, canvas.width, canvas.height);

    }

    public DrawAt(col: number, row: number, char: BtermCharacter ): void {
        const ctx = this._canvas.getContext('2d') as CanvasRenderingContext2D;
        const cWidth = 16;
        const cHeight = 16;
        ctx.fillStyle=`rgb(${char.back.r},${char.back.g},${char.back.b})`;
        ctx.fillRect(col * cWidth, row * cHeight, cWidth, cHeight);
        ctx.font = "12px Consolas";
        ctx.fillStyle = `rgb(${char.fore.r},${char.fore.g},${char.fore.b})`;
        ctx.fillText(String.fromCharCode(char.codePoint), col * cWidth, (row +1)* cHeight);
    }

    public static DrawAt(id: string, col: number, row: number, char: BtermCharacter): void {
        console.log('BTerm::DrawAt to id ' + id + ' with char ' + char.codePoint);
        GetBterm(id)?.DrawAt(col, row, char);
    }
}

export { Bterm, Attach, GetBterm };