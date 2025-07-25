import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { User, userFieldsConfiguration } from './model/user';

// TODO after creation of CRUD User : adapt the global configuration
export const userCRUDConfiguration: CrudConfig<User> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'users',
  fieldsConfig: userFieldsConfiguration,
  useCalcMode: false,
  useSignalR: false,
  useView: false,
  // useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  usePopup: true,
  useOfflineMode: false,
  importMode: {
    useInsert: true,
    useUpdate: true,
    useDelete: false,
  },
  showIcons: {
    showCalcMode: true,
    showPopup: true,
  },
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  // tableStateKey: featureName + 'Grid',
});
