import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { TableLazyLoadEvent } from 'primeng/table';
import { AircraftMaintenanceCompany } from '../model/aircraft-maintenance-company';
import { FeatureAircraftMaintenanceCompaniesActions } from './aircraft-maintenance-companies-actions';

// This adapter will allow is to manipulate aircraftMaintenanceCompanies (mostly CRUD operations)
export const aircraftMaintenanceCompaniesAdapter =
  createEntityAdapter<AircraftMaintenanceCompany>({
    selectId: (aircraftMaintenanceCompany: AircraftMaintenanceCompany) =>
      aircraftMaintenanceCompany.id,
    sortComparer: false,
  });

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<AircraftMaintenanceCompany> {
//   ids: string[] | number[];
//   entities: { [id: string]: AircraftMaintenanceCompany };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<AircraftMaintenanceCompany> {
  // additional props here
  totalCount: number;
  currentAircraftMaintenanceCompany: AircraftMaintenanceCompany;
  lastLazyLoadEvent: TableLazyLoadEvent;
  loadingGet: boolean;
  loadingGetAll: boolean;
}

export const INIT_STATE: State =
  aircraftMaintenanceCompaniesAdapter.getInitialState({
    // additional props default values here
    totalCount: 0,
    currentAircraftMaintenanceCompany: <AircraftMaintenanceCompany>{},
    lastLazyLoadEvent: <TableLazyLoadEvent>{},
    loadingGet: false,
    loadingGetAll: false,
  });

export const aircraftMaintenanceCompanyReducers = createReducer<State>(
  INIT_STATE,
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
        currentAircraftMaintenanceCompany: aircraftMaintenanceCompany,
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
