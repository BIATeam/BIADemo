import { Action, ActionReducerMap, MetaReducer } from '@ngrx/store';
import { InjectionToken } from '@angular/core';
import { environment } from 'src/environments/environment';
import { logger } from '../build-specifics/bia-build-specifics';

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface AppState {
  // more state here
}

// AOT compatibility
export const ROOT_REDUCERS = new InjectionToken<ActionReducerMap<AppState, Action>>('ROOT_REDUCERS_TOKEN', {
  factory: () => ({})
});



export const metaReducers: MetaReducer<AppState>[] = environment.production ? [] : [logger];
