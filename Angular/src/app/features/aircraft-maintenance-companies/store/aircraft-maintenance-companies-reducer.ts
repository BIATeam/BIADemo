import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import {
  CrudState,
  DEFAULT_CRUD_STATE,
} from 'packages/bia-ng/models/public-api';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { FeatureAircraftMaintenanceCompaniesActions } from './aircraft-maintenance-companies-actions';

// This adapter will allow is to manipulate aircraftMaintenanceCompanies (mostly CRUD operations)
export const aircraftMaintenanceCompaniesAdapter =
  createEntityAdapter<AircraftMaintenanceCompany>({
    selectId: (aircraftMaintenanceCompany: AircraftMaintenanceCompany) =>
      aircraftMaintenanceCompany.id,
    sortComparer: false,
  });

export interface State
  extends
    CrudState<AircraftMaintenanceCompany>,
    EntityState<AircraftMaintenanceCompany> {
  // additional props here
}

export const INIT_STATE: State =
  aircraftMaintenanceCompaniesAdapter.getInitialState({
    ...DEFAULT_CRUD_STATE(),
    // additional props default values here
  });

export const aircraftMaintenanceCompanyReducers = createReducer<State>(
  INIT_STATE,
  on(FeatureAircraftMaintenanceCompaniesActions.clearAll, state => {
    const stateUpdated = aircraftMaintenanceCompaniesAdapter.removeAll(state);
    stateUpdated.totalCount = 0;
    return stateUpdated;
  }),
  on(FeatureAircraftMaintenanceCompaniesActions.clearCurrent, state => {
    return { ...state, currentItem: <AircraftMaintenanceCompany>{} };
  }),
  on(FeatureAircraftMaintenanceCompaniesActions.loadAllByPost, state => {
    return { ...state, loadingGetAll: true };
  }),
  on(FeatureAircraftMaintenanceCompaniesActions.load, state => {
    return { ...state, loadingGet: true };
  }),
  on(
    FeatureAircraftMaintenanceCompaniesActions.loadAllByPostSuccess,
    (state, { result, event }) => {
      const stateUpdated = aircraftMaintenanceCompaniesAdapter.setAll(
        result.data,
        state
      );
      stateUpdated.totalCount = result.totalCount;
      stateUpdated.lastLazyLoadEvent = event;
      stateUpdated.loadingGetAll = false;
      return stateUpdated;
    }
  ),
  on(
    FeatureAircraftMaintenanceCompaniesActions.loadSuccess,
    (state, { aircraftMaintenanceCompany }) => {
      return {
        ...state,
        currentItem: aircraftMaintenanceCompany,
        loadingGet: false,
      };
    }
  ),
  on(FeatureAircraftMaintenanceCompaniesActions.failure, state => {
    return { ...state, loadingGetAll: false, loadingGet: false };
  })
);

export const getAircraftMaintenanceCompanyById =
  (id: number) => (state: State) =>
    state.entities[id];
