import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { TeamTypeId } from 'src/app/shared/constants';
import { planeFieldsConfiguration } from './model/plane';

// TODO after creation of CRUD Plane : adapt the global configuration
export const PlaneCRUDConfiguration: CrudConfig = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'planes',
  fieldsConfig: planeFieldsConfiguration,
  useCalcMode: true,
  useSignalR: false,
  useView: true,
  /// BIAToolKit - Begin AncestorTeam Site
  useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  /// BIAToolKit - End AncestorTeam Site
  usePopup: true,
  useOfflineMode: false,
  // Begin BIADemo
  bulkMode: {
    useInsert: true,
    useUpdate: true,
    useDelete: true,
  },
  // End BIADemo
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  // tableStateKey: featureName + 'Grid',
});
