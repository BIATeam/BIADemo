import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  FixableDto,
  VersionedDto,
} from '@bia-team/bia-ng/models';
import { PropType } from '@bia-team/bia-ng/models/enum';

// TODO after creation of CRUD Pilot : adapt the model
export interface Pilot extends BaseDto<string>, VersionedDto, FixableDto {
  siteId: number;
  identificationNumber: string;
  flightHours: number | null;
}

// TODO after creation of CRUD Pilot : adapt the field configuration
export const pilotFieldsConfiguration: BiaFieldsConfig<Pilot> = {
  columns: [
    Object.assign(
      new BiaFieldConfig('identificationNumber', 'pilot.identificationNumber'),
      {
        type: PropType.String,
        isRequired: true,
      }
    ),
    Object.assign(new BiaFieldConfig('flightHours', 'pilot.flightHours'), {
      type: PropType.Number,
    }),
    Object.assign(new BiaFieldConfig('rowVersion', 'pilot.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Pilot : adapt the form layout configuration
export const pilotFormLayoutConfiguration: BiaFormLayoutConfig<Pilot> =
  new BiaFormLayoutConfig([]);
