import { TvisionXtermBridge, bindTerminal} from './tvision-xterm'

window['tv$'] = Object.assign(TvisionXtermBridge, { bindTerminal: bindTerminal });
console.log('Tvision Blazor Module initialized');