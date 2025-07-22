import { Validators } from '@angular/forms';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldNumberFormat,
  BiaFieldsConfig,
  NumberMode,
  OptionDto,
  PrimeNGFiltering,
  PropType,
  VersionedDto,
} from 'biang/models';

export interface Engine extends BaseDto, VersionedDto {
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
    Object.assign(new BiaFieldConfig('syncTime', 'engine.syncTime'), {
      type: PropType.TimeSecOnly,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('ignitionTime', 'engine.ignitionTime'), {
      type: PropType.TimeSecOnly,
    }),
    Object.assign(new BiaFieldConfig('power', 'engine.power'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
    }),
    Object.assign(new BiaFieldConfig('noiseLevel', 'engine.noiseLevel'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('flightHours', 'engine.flightHours'), {
      isRequired: true,
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 6,
        maxFractionDigits: 6,
      }),
      validators: [Validators.min(0)],
    }),
    Object.assign(
      new BiaFieldConfig('averageFlightHours', 'engine.averageFlightHours'),
      {
        type: PropType.Number,
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 6,
          maxFractionDigits: 6,
        }),
      }
    ),
    Object.assign(
      new BiaFieldConfig('fuelConsumption', 'engine.fuelConsumption'),
      {
        isRequired: true,
        type: PropType.Number,
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 2,
          maxFractionDigits: 2,
        }),
        validators: [Validators.min(0)],
      }
    ),
    Object.assign(
      new BiaFieldConfig(
        'averageFuelConsumption',
        'engine.averageFuelConsumption'
      ),
      {
        type: PropType.Number,
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 2,
          maxFractionDigits: 2,
        }),
        validators: [Validators.min(0), Validators.max(100)],
      }
    ),
    Object.assign(new BiaFieldConfig('originalPrice', 'engine.originalPrice'), {
      isRequired: true,
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Currency,
        minFractionDigits: 2,
        maxFractionDigits: 2,
        currency: 'EUR',
      }),
      validators: [Validators.min(0)],
    }),
    Object.assign(
      new BiaFieldConfig('estimatedPrice', 'engine.estimatedPrice'),
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
    Object.assign(new BiaFieldConfig('isHybrid', 'engine.isHybrid'), {
      isSearchable: true,
      type: PropType.Boolean,
    }),
    Object.assign(
      new BiaFieldConfig('isToBeMaintained', 'engine.isToBeMaintained'),
      {
        isRequired: true,
        isSearchable: true,
        type: PropType.Boolean,
      }
    ),
    Object.assign(new BiaFieldConfig('principalPart', 'engine.principalPart'), {
      type: PropType.OneToMany,
    }),
    Object.assign(
      new BiaFieldConfig('installedParts', 'engine.installedParts'),
      {
        type: PropType.ManyToMany,
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'engine.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};
