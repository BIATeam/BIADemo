import { CrudConfig } from '@bia-team/bia-ng/shared';
import { PlaneType, planeTypeFieldsConfiguration } from './model/plane-type';

// TODO after creation of CRUD PlaneType : adapt the global configuration
export const planeTypeCRUDConfiguration: CrudConfig<PlaneType> = new CrudConfig(
  {
    // IMPORTANT: this key should be unique in all the application.
    featureName: 'planes-types',
    fieldsConfig: planeTypeFieldsConfiguration,
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
  }
);
