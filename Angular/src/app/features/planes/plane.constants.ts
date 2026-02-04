import { CrudConfig, FormReadOnlyMode } from '@bia-team/bia-ng/shared';
import { TeamTypeId } from 'src/app/shared/constants';
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
  formEditReadOnlyMode: FormReadOnlyMode.off,
  hasReadView: true,
  isFixable: true,
  displayHistorical: true,
  // Begin BIAToolKit Generation Ignore
  isCloneable: true,
  featureNameSingular: 'plane',
  // End BIAToolKit Generation Ignore
  useCalcMode: true,
  useSignalR: false,
  useView: true,
  useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  usePopup: false,
  useSplit: false,
  useOfflineMode: false,
  useCompactMode: false,
  useVirtualScroll: false,
  // Begin BIAToolKit Generation Ignore
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
  // End BIAToolKit Generation Ignore
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  // tableStateKey: featureName + 'Grid',
});
