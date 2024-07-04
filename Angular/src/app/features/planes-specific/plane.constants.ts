import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { TeamTypeId } from 'src/app/shared/constants';
import { planeFieldsConfiguration } from './model/plane';

// IMPORTANT: this key should be unique in all the application.
export const featureName = 'planes-specific';

// TODO after creation of CRUD Plane : adapt the global configuration
export const planeCRUDConfiguration: CrudConfig = new CrudConfig({
  featureName: 'planes-specific',
  fieldsConfig: planeFieldsConfiguration,
  useCalcMode: false,
  useSignalR: false,
  useView: false,
  useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  usePopup: true,
  useOfflineMode: false,
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  tableStateKey: 'planesGrid',
});
