import { Action, ActionReducerMap, ActionReducer, MetaReducer } from '@ngrx/store';
import { storeLogger } from 'ngrx-store-logger';
import { InjectionToken } from '@angular/core';
import { environment } from 'src/environments/environment';

// tslint:disable-next-line: no-empty-interface
export interface AppState {
  // more state here
}

// AOT compatibility
export const ROOT_REDUCERS = new InjectionToken<ActionReducerMap<AppState, Action>>('ROOT_REDUCERS_TOKEN', {
  factory: () => ({})
});

export function logger(reducer: ActionReducer<AppState>): any {
  return storeLogger()(reducer);
}

export const metaReducers: MetaReducer<AppState>[] = environment.production ? [] : [logger];
