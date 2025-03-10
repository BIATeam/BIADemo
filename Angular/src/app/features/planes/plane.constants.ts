import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
/// BIAToolKit - Begin AncestorTeam
import { TeamTypeId } from 'src/app/shared/constants';
/// BIAToolKit - End AncestorTeam
import {
  Plane,
  planeFieldsConfiguration,
  planeFormLayoutConfiguration,
} from './model/plane';

// TODO after creation of CRUD Plane : adapt the global configuration
export const planeCRUDConfiguration: CrudConfig<Plane> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'planes',
  fieldsConfig: planeFieldsConfiguration,
  formLayoutConfig: planeFormLayoutConfiguration,
  // Begin BIADemo
  isFixable: true,
  // End BIADemo
  useCalcMode: true,
  useSignalR: false,
  useView: true,
  /// BIAToolKit - Begin AncestorTeam Site
  useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  /// BIAToolKit - End AncestorTeam Site
  usePopup: false,
  useSplit: false,
  useOfflineMode: false,
  useCompactMode: false,
  useVirtualScroll: false,
  // Begin BIADemo
  importMode: {
    useInsert: true,
    useUpdate: true,
    useDelete: true,
  },
  showIcons: {
    showCalcMode: true,
    showPopup: true,
    showSplit: true,
    showView: true,
    showSignalR: true,
    showCompactMode: true,
    showVirtualScroll: true,
    showResizableColumn: true,
  },

  // End BIADemo
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  // tableStateKey: featureName + 'Grid',
});
