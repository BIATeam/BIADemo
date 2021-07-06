import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import {
  loadSuccess,
  loadAllByPostSuccess,
  loadAllByPost,
  load,
  openDialogEdit,
  closeDialogEdit,
  openDialogNew,
  closeDialogNew,
  failure
} from './members-actions';
import { LazyLoadEvent } from 'primeng/api';
import { Member } from '../model/member';

// This adapter will allow is to manipulate members (mostly CRUD operations)
export const membersAdapter = createEntityAdapter<Member>({
  selectId: (member: Member) => member.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Member> {
//   ids: string[] | number[];
//   entities: { [id: string]: Member };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<Member> {
  // additional props here
  totalCount: number;
  currentMember: Member;
  lastLazyLoadEvent: LazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
  displayEditDialog: boolean;
  displayNewDialog: boolean;
}

export const INIT_STATE: State = membersAdapter.getInitialState({
  // additional props default values here
  totalCount: 0,
  currentMember: <Member>{},
  lastLazyLoadEvent: <LazyLoadEvent>{},
  loadingGet: false,
  loadingGetAll: false,
  displayEditDialog: false,
  displayNewDialog: false
});

export const memberReducers = createReducer<State>(
  INIT_STATE,
  on(openDialogNew, (state) => {
    return { ...state, displayNewDialog: true };
  }),
  on(closeDialogNew, (state) => {
    return { ...state, displayNewDialog: false };
  }),
  on(openDialogEdit, (state) => {
    return { ...state, displayEditDialog: true };
  }),
  on(closeDialogEdit, (state) => {
    return { ...state, displayEditDialog: false };
  }),
  on(loadAllByPost, (state, { event }) => {
    return { ...state, loadingGetAll: true };
  }),
  on(load, (state) => {
    return { ...state, loadingGet: true };
  }),
  on(loadAllByPostSuccess, (state, { result, event }) => {
    const stateUpdated = membersAdapter.setAll(result.data, state);
    stateUpdated.currentMember = <Member>{};
    stateUpdated.totalCount = result.totalCount;
    stateUpdated.lastLazyLoadEvent = event;
    stateUpdated.loadingGetAll = false;
    return stateUpdated;
  }),
  on(loadSuccess, (state, { member }) => {
    return { ...state, currentMember: member, loadingGet: false };
  }),
  on(failure, (state, { error }) => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  }),
);

export const getMemberById = (id: number) => (state: State) => state.entities[id];
