import { CrudConfig } from 'packages/bia-ng/shared/public-api';
import { Engine, engineFieldsConfiguration } from '../../model/engine';

export const engineCRUDConfiguration: CrudConfig<Engine> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'engines-specific',
  fieldsConfig: engineFieldsConfiguration,
});
