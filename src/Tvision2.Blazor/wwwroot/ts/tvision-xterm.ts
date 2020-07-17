import { Terminal } from 'xterm';

const _terminal: Terminal = new Terminal();

function bindTerminal(id: string): void {
    _terminal.open(document.getElementById(id));
}


const TvisionXtermBridge = {
    write(data: string) {
        _terminal.write(data);
    }
}



export { TvisionXtermBridge, bindTerminal }


