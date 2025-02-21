import { Validators } from '@angular/forms';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldNumberFormat,
  BiaFieldsConfig,
  NumberMode,
  PrimeNGFiltering,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import {
  BiaFormConfig,
  BiaFormConfigColumn,
  BiaFormConfigGroup,
  BiaFormConfigRow,
} from 'src/app/shared/bia-shared/model/bia-form-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD Plane : adapt the model
export class Plane extends BaseDto {
  /// BIAToolKit - Begin Properties
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
  /// BIAToolKit - End Properties
}

export const planeFormConfiguration: BiaFormConfig<Plane> =
  new BiaFormConfig<Plane>([
    new BiaFormConfigGroup('Identification', [
      new BiaFormConfigRow([
        new BiaFormConfigColumn('msn'),
        new BiaFormConfigColumn('manufacturer'),
      ]),
    ]),
    new BiaFormConfigGroup('Status', [
      new BiaFormConfigRow([
        new BiaFormConfigColumn('isActive'),
        new BiaFormConfigColumn('isMaintenance'),
      ]),
    ]),
    new BiaFormConfigGroup('Tracking', [
      new BiaFormConfigRow([
        new BiaFormConfigColumn('deliveryDate'),
        new BiaFormConfigColumn('firstFlightDate'),
        new BiaFormConfigColumn('lastFlightDate'),
        new BiaFormConfigColumn('nextMaintenanceDate'),
      ]),
    ]),
    new BiaFormConfigRow([
      new BiaFormConfigColumn('syncTime'),
      new BiaFormConfigColumn('syncFlightDataTime'),
      new BiaFormConfigColumn('capacity'),
    ]),
  ]);

// TODO after creation of CRUD Plane : adapt the field configuration
export const planeFieldsConfiguration: BiaFieldsConfig<Plane> = {
  columns: [
    /// BIAToolKit - Begin Block msn
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
      validators: [Validators.required, Validators.maxLength(64)],
    }),
    /// BIAToolKit - End Block msn
    /// BIAToolKit - Begin Block manufacturer
    Object.assign(new BiaFieldConfig('manufacturer', 'plane.manufacturer'), {}),
    /// BIAToolKit - End Block manufacturer
    /// BIAToolKit - Begin Block isActive
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isRequired: true,
      isSearchable: true,
      // Begin BIADemo
      isSortable: false,
      // End BIADemo
      type: PropType.Boolean,
      validators: [Validators.required],
    }),
    /// BIAToolKit - End Block isActive
    /// BIAToolKit - Begin Block isMaintenance
    Object.assign(new BiaFieldConfig('isMaintenance', 'plane.isMaintenance'), {
      isSearchable: true,
      // Begin BIADemo
      isSortable: false,
      // End BIADemo
      type: PropType.Boolean,
    }),
    /// BIAToolKit - End Block isMaintenance
    /// BIAToolKit - Begin Block firstFlightDate
    Object.assign(
      new BiaFieldConfig('firstFlightDate', 'plane.firstFlightDate'),
      {
        isRequired: true,
        type: PropType.DateTime,
        validators: [Validators.required],
      }
    ),
    /// BIAToolKit - End Block firstFlightDate
    /// BIAToolKit - Begin Block lastFlightDate
    Object.assign(
      new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'),
      {
        type: PropType.DateTime,
      }
    ),
    /// BIAToolKit - End Block lastFlightDate
    /// BIAToolKit - Begin Block deliveryDate
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
    }),
    /// BIAToolKit - End Block deliveryDate
    /// BIAToolKit - Begin Block nextMaintenanceDate
    Object.assign(
      new BiaFieldConfig('nextMaintenanceDate', 'plane.nextMaintenanceDate'),
      {
        isRequired: true,
        type: PropType.Date,
        validators: [Validators.required],
      }
    ),
    /// BIAToolKit - End Block nextMaintenanceDate
    /// BIAToolKit - Begin Block syncTime
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
    }),
    /// BIAToolKit - End Block syncTime
    /// BIAToolKit - Begin Block syncFlightDataTime
    Object.assign(
      new BiaFieldConfig('syncFlightDataTime', 'plane.syncFlightDataTime'),
      {
        isRequired: true,
        type: PropType.TimeSecOnly,
        validators: [Validators.required],
      }
    ),
    /// BIAToolKit - End Block syncFlightDataTime
    /// BIAToolKit - Begin Block capacity
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
      validators: [Validators.required, Validators.min(1)],
    }),
    /// BIAToolKit - End Block capacity
    /// BIAToolKit - Begin Block motorsCount
    Object.assign(new BiaFieldConfig('motorsCount', 'plane.motorsCount'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
    }),
    /// BIAToolKit - End Block motorsCount
    /// BIAToolKit - Begin Block totalFlightHours
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
    /// BIAToolKit - End Block totalFlightHours
    /// BIAToolKit - Begin Block probability
    Object.assign(new BiaFieldConfig('probability', 'plane.probability'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 6,
        maxFractionDigits: 6,
      }),
    }),
    /// BIAToolKit - End Block probability
    /// BIAToolKit - Begin Block fuelCapacity
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
    /// BIAToolKit - End Block fuelCapacity
    /// BIAToolKit - Begin Block fuelLevel
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
    /// BIAToolKit - End Block fuelLevel
    /// BIAToolKit - Begin Block originalPrice
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
    /// BIAToolKit - End Block originalPrice
    /// BIAToolKit - Begin Block estimatedPrice
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
    /// BIAToolKit - End Block estimatedPrice
    /// BIAToolKit - Begin Block planeType
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
    }),
    /// BIAToolKit - End Block planeType
    /// BIAToolKit - Begin Block similarTypes
    Object.assign(new BiaFieldConfig('similarTypes', 'plane.similarTypes'), {
      type: PropType.ManyToMany,
    }),
    /// BIAToolKit - End Block similarTypes
    /// BIAToolKit - Begin Block currentAirport
    Object.assign(
      new BiaFieldConfig('currentAirport', 'plane.currentAirport'),
      {
        isRequired: true,
        type: PropType.OneToMany,
        validators: [Validators.required],
      }
    ),
    /// BIAToolKit - End Block currentAirport
    /// BIAToolKit - Begin Block connectingAirports
    Object.assign(
      new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'),
      {
        isRequired: true,
        type: PropType.ManyToMany,
        validators: [Validators.required],
      }
    ),
    /// BIAToolKit - End Block connectingAirports
    Object.assign(new BiaFieldConfig('rowVersion', 'plane.rowVersion'), {
      isVisible: false,
      isHideByDefault: true,
    }),
  ],
};
