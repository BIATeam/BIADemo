import { ActionReducer } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { storeLogger } from 'ngrx-store-logger';
import { environment } from 'src/environments/environment';
import { AppState } from '../../store/state';


// the Ngrx Store DevTools is to use with the chrome extension Redux Devtools Extension (https://github.com/reduxjs/redux-devtools)
export const buildSpecificModules = [
    StoreDevtoolsModule.instrument({
        maxAge: 25, // Retains last 25 states
        logOnly: environment.production, // Restrict extension to log-only mode
        autoPause: true, // Pauses recording actions and state changes when the extension window is not open
      }),
];

// The Ngrx Store Logger log the state in the console
export function logger(reducer: ActionReducer<AppState>): any {
    return storeLogger()(reducer);
  }