import { CrudConfig } from '@bia-team/bia-ng/shared';
import { TeamTypeId } from 'src/app/shared/constants';
import {
  Engine,
  engineFieldsConfiguration,
  engineFormLayoutConfiguration,
} from './model/engine';

// TODO after creation of CRUD Engine : adapt the global configuration
export const engineCRUDConfiguration: CrudConfig<Engine> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'engines',
  fieldsConfig: engineFieldsConfiguration,
  formLayoutConfig: engineFormLayoutConfiguration,
  isFixable: true,
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
    useDelete: false,
  },
  // End BIAToolKit Generation Ignore
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  // tableStateKey: featureName + 'Grid',
});
