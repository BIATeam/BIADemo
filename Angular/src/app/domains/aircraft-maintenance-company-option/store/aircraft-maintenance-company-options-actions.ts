import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'packages/bia-ng/models/public-api';
import { storeKey } from '../aircraft-maintenance-company-option.contants';

export namespace DomainAircraftMaintenanceCompanyOptionsActions {
  export const loadAll = createAction('[' + storeKey + '] Load all');

  export const loadAllSuccess = createAction(
    '[' + storeKey + '] Load all success',
    props<{ aircraftMaintenanceCompanies: OptionDto[] }>()
  );

  export const failure = createAction(
    '[' + storeKey + '] Failure',
    props<{ error: any }>()
  );
}
