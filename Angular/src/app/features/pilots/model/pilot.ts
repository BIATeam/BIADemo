import { OptionDto } from '@bia-team/bia-ng/models';
import { PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  FixableDto,
  VersionedDto,
} from 'packages/bia-ng/models/public-api';

// TODO after creation of CRUD Pilot : adapt the model
export interface Pilot extends BaseDto<string>, VersionedDto, FixableDto {
  siteId: number;
  identificationNumber: string;
  firstName: string;
  lastName: string;
  birthdate: Date | null;
  cplDate: Date;
  baseAirport: OptionDto | null;
  flightHours: number;
  firstFlightDate: Date;
  lastFlightDate: Date | null;
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
    Object.assign(new BiaFieldConfig('firstName', 'pilot.firstName'), {
      type: PropType.String,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('lastName', 'pilot.lastName'), {
      type: PropType.String,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('birthdate', 'pilot.birthdate'), {
      type: PropType.Date,
    }),
    Object.assign(new BiaFieldConfig('cplDate', 'pilot.cplDate'), {
      type: PropType.Date,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('baseAirport', 'pilot.baseAirport'), {
      type: PropType.OneToMany,
    }),
    Object.assign(new BiaFieldConfig('flightHours', 'pilot.flightHours'), {
      type: PropType.Number,
      isRequired: true,
    }),
    Object.assign(
      new BiaFieldConfig('firstFlightDate', 'pilot.firstFlightDate'),
      {
        type: PropType.DateTime,
        isRequired: true,
        asLocalDateTime: true,
      }
    ),
    Object.assign(
      new BiaFieldConfig('lastFlightDate', 'pilot.lastFlightDate'),
      {
        type: PropType.DateTime,
        asLocalDateTime: true,
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'pilot.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Pilot : adapt the form layout configuration
export const pilotFormLayoutConfiguration: BiaFormLayoutConfig<Pilot> =
  new BiaFormLayoutConfig([]);
