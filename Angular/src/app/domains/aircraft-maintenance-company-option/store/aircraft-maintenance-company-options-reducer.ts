import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { DomainAircraftMaintenanceCompanyOptionsActions } from './aircraft-maintenance-company-options-actions';

// This adapter will allow is to manipulate aircraftMaintenanceCompanies (mostly CRUD operations)
export const aircraftMaintenanceCompanyOptionsAdapter =
  createEntityAdapter<OptionDto>({
    selectId: (aircraftMaintenanceCompany: OptionDto) =>
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

export type State = EntityState<OptionDto>;

export const INIT_STATE: State =
  aircraftMaintenanceCompanyOptionsAdapter.getInitialState({
    // additional props default values here
  });

export const aircraftMaintenanceCompanyOptionReducers = createReducer<State>(
  INIT_STATE,
  on(
    DomainAircraftMaintenanceCompanyOptionsActions.loadAllSuccess,
    (state, { aircraftMaintenanceCompanies }) =>
      aircraftMaintenanceCompanyOptionsAdapter.setAll(
        aircraftMaintenanceCompanies,
        state
      )
  )
  // on(loadSuccess, (state, { aircraftMaintenanceCompany }) => aircraftMaintenanceCompanyOptionsAdapter.upsertOne(aircraftMaintenanceCompany, state))
);

export const getAircraftMaintenanceCompanyOptionById =
  (id: number) => (state: State) =>
    state.entities[id];
