﻿import { CrudConfig } from 'biang/shared';
import { Engine, engineFieldsConfiguration } from '../../model/engine';

export const engineCRUDConfiguration: CrudConfig<Engine> = new CrudConfig({
  // IMPORTANT: this key should be unique in all the application.
  featureName: 'engines-specific',
  fieldsConfig: engineFieldsConfiguration,
});
