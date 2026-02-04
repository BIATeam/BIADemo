import { CrudConfig, FormReadOnlyMode } from '@bia-team/bia-ng/shared';
import { TeamTypeId } from 'src/app/shared/constants';
import { Plane, planeFieldsConfiguration } from './model/plane';
import {
  PlaneSpecific,
  planeSpecificFieldsConfiguration,
  planeSpecificFormLayoutConfiguration,
} from './model/plane-specific';

// IMPORTANT: this key should be unique in all the application.
export const featureName = 'planes-specific';

// TODO after creation of CRUD Plane : adapt the global configuration
export const planeCRUDConfiguration: CrudConfig<Plane> = new CrudConfig({
  featureName: 'planes-specific',
  fieldsConfig: planeFieldsConfiguration,
  formEditReadOnlyMode: FormReadOnlyMode.clickToEdit,
  hasReadView: true,
  useCalcMode: false,
  useSignalR: true,
  useView: false,
  useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  usePopup: true,
  useOfflineMode: false,
  // IMPORTANT: this key should be unique in all the application.
  // storeKey: 'feature-' + featureName,
  // IMPORTANT: this is the key used for the view management it should be unique in all the application (except if share same views).
  tableStateKey: 'planesGrid',
  useCompactMode: true,
  useVirtualScroll: false,
  showIcons: {
    showPopup: true,
    showView: true,
    showCompactMode: true,
    showResizableColumn: true,
  },
});

export const planeSpecificCRUDConfiguration: CrudConfig<PlaneSpecific> =
  new CrudConfig({
    ...planeCRUDConfiguration,
    fieldsConfig: planeSpecificFieldsConfiguration,
    formLayoutConfig: planeSpecificFormLayoutConfiguration,
  });
