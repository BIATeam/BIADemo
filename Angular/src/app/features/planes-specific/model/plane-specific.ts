import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { Engine } from './engine';
import { Plane, planeFieldsConfiguration } from './plane';

// TODO after creation of CRUD Plane : adapt the model
export interface PlaneSpecific extends Plane {
  engines: Engine[];
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const planeSpecificFieldsConfiguration: BiaFieldsConfig = {
  columns: [
    ...planeFieldsConfiguration.columns,
    Object.assign(new BiaFieldConfig('engines', 'plane.engines'), {
      specificOutput: true,
      specificInput: true,
      minWidth: '50px',
      type: PropType.ManyToMany,
    }),
  ],
};
