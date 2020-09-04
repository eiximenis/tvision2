import { Terminal } from 'xterm';

const _terminal: Terminal = new Terminal();
let _dotnetRef: any | null = null;

const TvisionXtermBridge = {
    write(data: string) {
        _terminal.write(data);
    }
}


function bindTerminal(id: string, dotnetRef: any): void {
    _terminal.open(document.getElementById(id));
    _dotnetRef = dotnetRef;

    _terminal.onKey(key => _dotnetRef.invokeMethodAsync('OnKeyDown', key.key));
    _terminal.onResize((cols, rows) => _dotnetRef.invokeMethodAsync('OnResize', cols, rows));
}

export { TvisionXtermBridge, bindTerminal }


