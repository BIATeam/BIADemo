import { Validators } from '@angular/forms';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  BiaFieldsConfig,
  NumberMode,
  PrimeNGFiltering,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import {
  BiaFormLayoutConfig,
  BiaFormLayoutConfigColumnSize,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigGroup,
  BiaFormLayoutConfigRow,
} from 'src/app/shared/bia-shared/model/bia-form-layout-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD Plane : adapt the model
export class Plane extends BaseDto {
  msn: string;
  manufacturer: string;
  isActive: boolean;
  isMaintenance: boolean | null;
  firstFlightDate: Date;
  lastFlightDate: Date | null;
  deliveryDate: Date | null;
  nextMaintenanceDate: Date;
  syncTime: string;
  syncFlightDataTime: string;
  capacity: number;
  motorsCount: number | null;
  totalFlightHours: number;
  probability: number | null;
  fuelCapacity: number;
  fuelLevel: number | null;
  originalPrice: number;
  estimatedPrice: number | null;
  planeType: OptionDto | null;
  similarTypes: OptionDto[];
  siteId: number;
  connectingAirports: OptionDto[];
  currentAirport: OptionDto;
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const planeFieldsConfiguration: BiaFieldsConfig<Plane> = {
  columns: [
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
      validators: [Validators.required, Validators.maxLength(64)],
    }),
    Object.assign(new BiaFieldConfig('manufacturer', 'plane.manufacturer'), {}),
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isRequired: true,
      isSearchable: true,
      // Begin BIADemo
      isSortable: false,
      // End BIADemo
      type: PropType.Boolean,
      validators: [Validators.required],
    }),
    Object.assign(new BiaFieldConfig('isMaintenance', 'plane.isMaintenance'), {
      isSearchable: true,
      // Begin BIADemo
      isSortable: false,
      // End BIADemo
      type: PropType.Boolean,
    }),
    Object.assign(
      new BiaFieldConfig('firstFlightDate', 'plane.firstFlightDate'),
      {
        isRequired: true,
        type: PropType.DateTime,
        validators: [Validators.required],
      }
    ),
    Object.assign(
      new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'),
      {
        type: PropType.DateTime,
        //Begin BIADemo
        displayFormat: Object.assign(new BiaFieldDateFormat(), {
          autoFormatDate: 'yyyy',
          autoPrimeDateFormat: 'yyyy',
        }),
        //End BIADemo
      }
    ),
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
    }),
    Object.assign(
      new BiaFieldConfig('nextMaintenanceDate', 'plane.nextMaintenanceDate'),
      {
        isRequired: true,
        type: PropType.Date,
        validators: [Validators.required],
      }
    ),
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
    }),
    Object.assign(
      new BiaFieldConfig('syncFlightDataTime', 'plane.syncFlightDataTime'),
      {
        isRequired: true,
        type: PropType.TimeSecOnly,
        validators: [Validators.required],
      }
    ),
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
      validators: [Validators.required, Validators.min(1)],
    }),
    Object.assign(new BiaFieldConfig('motorsCount', 'plane.motorsCount'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
    }),
    Object.assign(
      new BiaFieldConfig('totalFlightHours', 'plane.totalFlightHours'),
      {
        isRequired: true,
        type: PropType.Number,
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 6,
          maxFractionDigits: 6,
        }),
        validators: [Validators.required, Validators.min(0)],
      }
    ),
    Object.assign(new BiaFieldConfig('probability', 'plane.probability'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 6,
        maxFractionDigits: 6,
      }),
    }),
    Object.assign(new BiaFieldConfig('fuelCapacity', 'plane.fuelCapacity'), {
      isRequired: true,
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 2,
        maxFractionDigits: 2,
      }),
      validators: [Validators.required, Validators.min(0)],
    }),
    Object.assign(new BiaFieldConfig('fuelLevel', 'plane.fuelLevel'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 2,
        maxFractionDigits: 2,
      }),
      validators: [Validators.min(0), Validators.max(100)],
    }),
    Object.assign(new BiaFieldConfig('originalPrice', 'plane.originalPrice'), {
      isRequired: true,
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Currency,
        minFractionDigits: 2,
        maxFractionDigits: 2,
        currency: 'EUR',
      }),
      validators: [Validators.required, Validators.min(0)],
    }),
    Object.assign(
      new BiaFieldConfig('estimatedPrice', 'plane.estimatedPrice'),
      {
        type: PropType.Number,
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Currency,
          minFractionDigits: 2,
          maxFractionDigits: 2,
          currency: 'EUR',
        }),
        validators: [Validators.min(0)],
      }
    ),
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
    }),
    Object.assign(new BiaFieldConfig('similarTypes', 'plane.similarTypes'), {
      type: PropType.ManyToMany,
    }),
    Object.assign(
      new BiaFieldConfig('currentAirport', 'plane.currentAirport'),
      {
        isRequired: true,
        type: PropType.OneToMany,
        validators: [Validators.required],
      }
    ),
    Object.assign(
      new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'),
      {
        isRequired: true,
        type: PropType.ManyToMany,
        validators: [Validators.required],
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'plane.rowVersion'), {
      isVisible: false,
      isHideByDefault: true,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Plane : adapt the form layout configuration
export const planeFormLayoutConfiguration: BiaFormLayoutConfig<Plane> =
  new BiaFormLayoutConfig([
    //Begin BIADemo
    new BiaFormLayoutConfigRow([
      new BiaFormLayoutConfigGroup('plane.groupIdentification', [
        new BiaFormLayoutConfigRow([
          new BiaFormLayoutConfigField('msn'),
          new BiaFormLayoutConfigField('manufacturer'),
        ]),
      ]),
      new BiaFormLayoutConfigGroup('plane.groupStatus', [
        new BiaFormLayoutConfigRow([
          new BiaFormLayoutConfigField('isActive'),
          new BiaFormLayoutConfigField('isMaintenance'),
        ]),
      ]),
    ]),
    new BiaFormLayoutConfigGroup('plane.groupTracking', [
      new BiaFormLayoutConfigRow([
        new BiaFormLayoutConfigField('deliveryDate'),
        new BiaFormLayoutConfigField('firstFlightDate'),
        new BiaFormLayoutConfigField('lastFlightDate'),
        new BiaFormLayoutConfigField('nextMaintenanceDate'),
      ]),
      new BiaFormLayoutConfigRow([
        new BiaFormLayoutConfigField('syncFlightDataTime'),
        new BiaFormLayoutConfigField('syncTime'),
      ]),
    ]),
    new BiaFormLayoutConfigRow([
      new BiaFormLayoutConfigField(
        'motorsCount',
        new BiaFormLayoutConfigColumnSize(6, 6, 6, 6)
      ),
    ]),
    new BiaFormLayoutConfigRow([
      new BiaFormLayoutConfigField('probability'),
      new BiaFormLayoutConfigField('capacity'),
    ]),
    //End BIADemo
  ]);
