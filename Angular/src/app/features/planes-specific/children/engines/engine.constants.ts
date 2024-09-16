import { CrudConfig } from 'src/app/shared/bia-shared/feature-templates/crud-items/model/crud-config';
import { engineFieldsConfiguration } from '../../model/engine';

export const engineCRUDConfiguration: CrudConfig = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'engines-specific',
  fieldsConfig: engineFieldsConfiguration,
});
