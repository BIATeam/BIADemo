import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PrimeNGFiltering,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';

// TODO after creation of CRUD Engine : adapt the model
export interface Engine extends BaseDto {
  reference: string;
  lastMaintenanceDate: Date;
  syncTime: string;
  power: number;
  planeId: number;
}

// TODO after creation of CRUD Engine : adapt the field configuration
export const engineFieldsConfiguration: BiaFieldsConfig<Engine> = {
  columns: [
    Object.assign(new BiaFieldConfig('reference', 'engine.reference'), {}),
    Object.assign(
      new BiaFieldConfig('lastMaintenanceDate', 'engine.lastMaintenanceDate'),
      {
        type: PropType.DateTime,
        isRequired: true,
      }
    ),
    Object.assign(new BiaFieldConfig('syncTime', 'engine.syncTime'), {
      type: PropType.TimeSecOnly,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('power', 'engine.power'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
    }),
    Object.assign(new BiaFieldConfig('rowVersion', 'engine.rowVersion'), {
      isVisible: false,
      isHideByDefault: true,
    }),
  ],
};
