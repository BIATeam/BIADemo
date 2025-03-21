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
export interface Engine extends BaseDto {
  /// BIAToolKit - Begin Properties
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
  /// BIAToolKit - End Properties
}

// TODO after creation of CRUD Engine : adapt the field configuration
export const engineFieldsConfiguration: BiaFieldsConfig<Engine> = {
  columns: [
    /// BIAToolKit - Begin Block reference
    Object.assign(new BiaFieldConfig('reference', 'engine.reference'), {
      type: PropType.String,
      isRequired: true,
      validators: [Validators.required],
    }),
    /// BIAToolKit - End Block reference
    /// BIAToolKit - Begin Block manufacturer
    Object.assign(new BiaFieldConfig('manufacturer', 'engine.manufacturer'), {
      type: PropType.String,
    }),
    /// BIAToolKit - End Block manufacturer
    /// BIAToolKit - Begin Block nextMaintenanceDate
    Object.assign(
      new BiaFieldConfig('nextMaintenanceDate', 'engine.nextMaintenanceDate'),
      {
        type: PropType.DateTime,
        isRequired: true,
        validators: [Validators.required],
      }
    ),
    /// BIAToolKit - End Block nextMaintenanceDate
    /// BIAToolKit - Begin Block lastMaintenanceDate
    Object.assign(
      new BiaFieldConfig('lastMaintenanceDate', 'engine.lastMaintenanceDate'),
      {
        type: PropType.DateTime,
      }
    ),
    /// BIAToolKit - End Block lastMaintenanceDate
    /// BIAToolKit - Begin Block syncTime
    Object.assign(new BiaFieldConfig('syncTime', 'engine.syncTime'), {
      type: PropType.TimeSecOnly,
      isRequired: true,
      validators: [Validators.required],
    }),
    /// BIAToolKit - End Block syncTime
    /// BIAToolKit - Begin Block ignitionTime
    Object.assign(new BiaFieldConfig('ignitionTime', 'engine.ignitionTime'), {
      type: PropType.TimeSecOnly,
    }),
    /// BIAToolKit - End Block ignitionTime
    /// BIAToolKit - End Block power
    Object.assign(new BiaFieldConfig('power', 'engine.power'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
    }),
    /// BIAToolKit - Begin Block power
    /// BIAToolKit - End Block noiseLevel
    Object.assign(new BiaFieldConfig('noiseLevel', 'engine.noiseLevel'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
      validators: [Validators.required],
    }),
    /// BIAToolKit - Begin Block noiseLevel
    /// BIAToolKit - Begin Block flightHours
    Object.assign(new BiaFieldConfig('flightHours', 'engine.flightHours'), {
      isRequired: true,
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 6,
        maxFractionDigits: 6,
      }),
      validators: [Validators.required, Validators.min(0)],
    }),
    /// BIAToolKit - End Block flightHours
    /// BIAToolKit - Begin Block averageFlightHours
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
    /// BIAToolKit - End Block averageFlightHours
    /// BIAToolKit - Begin Block fuelConsumption
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
        validators: [Validators.required, Validators.min(0)],
      }
    ),
    /// BIAToolKit - End Block fuelConsumption
    /// BIAToolKit - Begin Block averageFuelConsumption
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
    /// BIAToolKit - End Block averageFuelConsumption
    /// BIAToolKit - Begin Block originalPrice
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
      validators: [Validators.required, Validators.min(0)],
    }),
    /// BIAToolKit - End Block originalPrice
    /// BIAToolKit - Begin Block estimatedPrice
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
    /// BIAToolKit - End Block estimatedPrice
    /// BIAToolKit - Begin Block isHybrid
    Object.assign(new BiaFieldConfig('isHybrid', 'engine.isHybrid'), {
      isSearchable: true,
      type: PropType.Boolean,
    }),
    /// BIAToolKit - End Block isHybrid
    /// BIAToolKit - Begin Block isToBeMaintained
    Object.assign(
      new BiaFieldConfig('isToBeMaintained', 'engine.isToBeMaintained'),
      {
        isRequired: true,
        isSearchable: true,
        type: PropType.Boolean,
        validators: [Validators.required],
      }
    ),
    /// BIAToolKit - End Block isToBeMaintained
    /// BIAToolKit - Begin Block principalPart
    Object.assign(new BiaFieldConfig('principalPart', 'engine.principalPart'), {
      type: PropType.OneToMany,
    }),
    /// BIAToolKit - End Block principalPart
    /// BIAToolKit - Begin Block installedParts
    Object.assign(
      new BiaFieldConfig('installedParts', 'engine.installedParts'),
      {
        type: PropType.ManyToMany,
      }
    ),
    /// BIAToolKit - End Block installedParts
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
