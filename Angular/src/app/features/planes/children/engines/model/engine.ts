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
import { BiaFormLayoutConfig } from 'src/app/shared/bia-shared/model/bia-form-layout-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD Engine : adapt the model
export class Engine extends BaseDto {
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
      validators: [Validators.required],
    }),
    Object.assign(new BiaFieldConfig('manufacturer', 'engine.manufacturer'), {
      type: PropType.String,
    }),
    Object.assign(
      new BiaFieldConfig('nextMaintenanceDate', 'engine.nextMaintenanceDate'),
      {
        type: PropType.DateTime,
        isRequired: true,
        validators: [Validators.required],
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
      validators: [Validators.required],
    }),
    Object.assign(new BiaFieldConfig('exchangeDate', 'engine.exchangeDate'), {
      type: PropType.Date,
    }),
    Object.assign(new BiaFieldConfig('syncTime', 'engine.syncTime'), {
      type: PropType.TimeSecOnly,
      isRequired: true,
      validators: [Validators.required],
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
      validators: [Validators.required],
    }),
    Object.assign(new BiaFieldConfig('flightHours', 'engine.flightHours'), {
      type: PropType.Number,
      isRequired: true,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 6,
        maxFractionDigits: 6,
      }),
      validators: [Validators.required, Validators.min(0)],
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
        type: PropType.Number,
        isRequired: true,
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 2,
          maxFractionDigits: 2,
        }),
        validators: [Validators.required, Validators.min(0)],
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
      type: PropType.Number,
      isRequired: true,
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
    Object.assign(
      new BiaFieldConfig('isToBeMaintained', 'engine.isToBeMaintained'),
      {
        type: PropType.Boolean,
        isRequired: true,
        isSearchable: true,
        validators: [Validators.required],
      }
    ),
    Object.assign(new BiaFieldConfig('isHybrid', 'engine.isHybrid'), {
      type: PropType.Boolean,
      isSearchable: true,
    }),
    Object.assign(new BiaFieldConfig('principalPart', 'engine.principalPart'), {
      type: PropType.OneToMany,
    }),
    Object.assign(
      new BiaFieldConfig('installedParts', 'engine.installedParts'),
      {
        type: PropType.ManyToMany,
        isRequired: true,
        validators: [Validators.required],
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'engine.rowVersion'), {
      isVisible: false,
      isHideByDefault: true,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Engine : adapt the form layout configuration
export const engineFormLayoutConfiguration: BiaFormLayoutConfig<Engine> =
  new BiaFormLayoutConfig([]);
