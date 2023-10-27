import { Action, ActionReducerMap, MetaReducer } from '@ngrx/store';
import { InjectionToken } from '@angular/core';

export interface AppState {
  // more state here
}

// AOT compatibility
export const ROOT_REDUCERS = new InjectionToken<ActionReducerMap<AppState, Action>>('ROOT_REDUCERS_TOKEN', {
  factory: () => ({})
});



export const metaReducers: MetaReducer<AppState>[] = [];
