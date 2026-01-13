import { CrudConfig } from '@bia-team/bia-ng/shared';
import { Site, siteFieldsConfiguration } from './model/site';

// TODO after creation of CRUD Team Site : adapt the global configuration
export const siteCRUDConfiguration: CrudConfig<Site> = new CrudConfig<Site>({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'sites',
  fieldsConfig: siteFieldsConfiguration,
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
