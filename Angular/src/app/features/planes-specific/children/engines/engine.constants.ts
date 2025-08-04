import { CrudConfig } from 'bia-ng/shared';
import { Engine, engineFieldsConfiguration } from '../../model/engine';

export const engineCRUDConfiguration: CrudConfig<Engine> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'engines-specific',
  fieldsConfig: engineFieldsConfiguration,
});
