import { CrudConfig } from '@bia-team/bia-ng/shared';
import { Airport, airportFieldsConfiguration } from './model/airport';

// TODO after creation of CRUD Airport : adapt the global configuration
export const airportCRUDConfiguration: CrudConfig<Airport> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'airports',
  fieldsConfig: airportFieldsConfiguration,
  useCalcMode: false,
  useSignalR: true,
  useView: false,
  // useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  usePopup: true,
  useOfflineMode: false,
  // IMPORTANT: this key should be unique in all the application. By default, if storeKey is not specified, value will be 'feature-' + featureName.
  // If you want to personalize it, uncomment next line
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  // tableStateKey: featureName + 'Grid',
});
