import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { memberFieldsConfiguration } from './model/member';

// TODO after creation of CRUD Member : adapt the global configuration
export const memberCRUDConfiguration: CrudConfig = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'members',
  fieldsConfig: memberFieldsConfiguration,
  useCalcMode: false,
  useSignalR: false,
  useView: false,
  // useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  usePopup: true,
  useOfflineMode: false,
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  // tableStateKey: featureName + 'Grid',
});
