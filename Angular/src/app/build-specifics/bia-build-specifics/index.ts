import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from 'src/environments/environment';

// the Ngrx Store DevTools is to use with the chrome extension Redux Devtools Extension (https://github.com/reduxjs/redux-devtools)
export const buildSpecificModules = [
  StoreDevtoolsModule.instrument({
    maxAge: 25, // Retains last 25 states
    logOnly: environment.production, // Restrict extension to log-only mode
    autoPause: true, // Pauses recording actions and state changes when the extension window is not open
  }),
];
