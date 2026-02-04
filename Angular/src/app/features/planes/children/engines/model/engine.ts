import { Validators } from '@angular/forms';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldNumberFormat,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  FixableDto,
  OptionDto,
  VersionedDto,
} from '@bia-team/bia-ng/models';
import {
  NumberMode,
  PrimeNGFiltering,
  PropType,
} from '@bia-team/bia-ng/models/enum';

// TODO after creation of CRUD Engine : adapt the model
export interface Engine extends BaseDto, VersionedDto, FixableDto {
  reference: string;
  manufacturer: string | null;
  nextMaintenanceDate: Date;
  lastMaintenanceDate: Date | null;
  deliveryDate: Date;
  exchangeDate: Date | null;
  syncTime: string;
  ignitionTime: string | null;
  power: number | null;
  noiseLevel: number;
  flightHours: number;
  averageFlightHours: number | null;
  fuelConsumption: number;
  averageFuelConsumption: number | null;
  originalPrice: number;
  estimatedPrice: number | null;
  isToBeMaintained: boolean;
  isHybrid: boolean | null;
  planeId: number;
  principalPart: OptionDto | null;
  installedParts: OptionDto[];
}

// TODO after creation of CRUD Engine : adapt the field configuration
export const engineFieldsConfiguration: BiaFieldsConfig<Engine> = {
  columns: [
    Object.assign(new BiaFieldConfig('reference', 'engine.reference'), {
      type: PropType.String,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('manufacturer', 'engine.manufacturer'), {
      type: PropType.String,
    }),
    Object.assign(
      new BiaFieldConfig('nextMaintenanceDate', 'engine.nextMaintenanceDate'),
      {
        type: PropType.DateTime,
        isRequired: true,
      }
    ),
    Object.assign(
      new BiaFieldConfig('lastMaintenanceDate', 'engine.lastMaintenanceDate'),
      {
        type: PropType.DateTime,
      }
    ),
    Object.assign(new BiaFieldConfig('deliveryDate', 'engine.deliveryDate'), {
      type: PropType.Date,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('exchangeDate', 'engine.exchangeDate'), {
      type: PropType.Date,
    }),
    Object.assign(new BiaFieldConfig('syncTime', 'engine.syncTime'), {
      type: PropType.TimeSecOnly,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('ignitionTime', 'engine.ignitionTime'), {
      type: PropType.TimeSecOnly,
    }),
    Object.assign(new BiaFieldConfig('power', 'engine.power'), {
      type: PropType.Number,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('noiseLevel', 'engine.noiseLevel'), {
      type: PropType.Number,
      isRequired: true,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('flightHours', 'engine.flightHours'), {
      type: PropType.Number,
      isRequired: true,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 6,
        maxFractionDigits: 6,
      }),
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(
      new BiaFieldConfig('averageFlightHours', 'engine.averageFlightHours'),
      {
        type: PropType.Number,
        // Begin BIAToolKit Generation Ignore
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 6,
          maxFractionDigits: 6,
        }),
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(
      new BiaFieldConfig('fuelConsumption', 'engine.fuelConsumption'),
      {
        type: PropType.Number,
        isRequired: true,
        // Begin BIAToolKit Generation Ignore
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 2,
          maxFractionDigits: 2,
        }),
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(
      new BiaFieldConfig(
        'averageFuelConsumption',
        'engine.averageFuelConsumption'
      ),
      {
        type: PropType.Number,
        // Begin BIAToolKit Generation Ignore
        validators: [Validators.min(0)],
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 2,
          maxFractionDigits: 2,
        }),
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(new BiaFieldConfig('originalPrice', 'engine.originalPrice'), {
      type: PropType.Number,
      isRequired: true,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Currency,
        minFractionDigits: 2,
        maxFractionDigits: 2,
        currency: 'EUR',
      }),
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(
      new BiaFieldConfig('estimatedPrice', 'engine.estimatedPrice'),
      {
        type: PropType.Number,
        // Begin BIAToolKit Generation Ignore
        validators: [Validators.min(0)],
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Currency,
          minFractionDigits: 2,
          maxFractionDigits: 2,
          currency: 'EUR',
        }),
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(
      new BiaFieldConfig('isToBeMaintained', 'engine.isToBeMaintained'),
      {
        type: PropType.Boolean,
        isRequired: true,
        // Begin BIAToolKit Generation Ignore
        isSearchable: true,
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(new BiaFieldConfig('isHybrid', 'engine.isHybrid'), {
      type: PropType.Boolean,
      // Begin BIAToolKit Generation Ignore
      isSearchable: true,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('principalPart', 'engine.principalPart'), {
      type: PropType.OneToMany,
    }),
    Object.assign(
      new BiaFieldConfig('installedParts', 'engine.installedParts'),
      {
        type: PropType.ManyToMany,
        isRequired: true,
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'engine.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Engine : adapt the form layout configuration
export const engineFormLayoutConfiguration: BiaFormLayoutConfig<Engine> =
  new BiaFormLayoutConfig([]);
