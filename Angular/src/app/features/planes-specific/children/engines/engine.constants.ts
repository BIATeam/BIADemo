import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { TeamTypeId } from 'src/app/shared/constants';
import { engineFieldsConfiguration } from '../../model/engine';

export const engineCRUDConfiguration: CrudConfig = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'engines',
  fieldsConfig: engineFieldsConfiguration,
  useCalcMode: true,
  useSignalR: false,
  useView: true,
  useViewTeamWithTypeId: TeamTypeId.Site, // use to filter view by teams => should know the type of team
  usePopup: true,
  useOfflineMode: false,
});
