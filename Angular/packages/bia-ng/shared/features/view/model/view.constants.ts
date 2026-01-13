import {
  CrudConfig,
  FormReadOnlyMode,
} from '../../../feature-templates/crud-items/model/crud-config';
import {
  View,
  viewFieldsConfiguration,
  viewFormLayoutConfiguration,
} from './view';

export const QUERY_STRING_VIEW = 'view';

export const viewCRUDConfiguration: CrudConfig<View> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'views',
  fieldsConfig: viewFieldsConfiguration,
  formLayoutConfig: viewFormLayoutConfiguration,
  formEditReadOnlyMode: FormReadOnlyMode.off,
  hasReadView: true,
  isFixable: true,
  displayHistorical: false,
  // Begin BIAToolKit Generation Ignore
  isCloneable: true,
  featureNameSingular: 'view',
  // End BIAToolKit Generation Ignore
  useCalcMode: false,
  useSignalR: false,
  useView: true,
  usePopup: false,
  useSplit: true,
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
    showCalcMode: false,
    showPopup: false,
    showSplit: false,
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
