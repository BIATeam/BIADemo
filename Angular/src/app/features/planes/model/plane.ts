import {
  PrimeNGFiltering,
  BiaFieldConfig,
  PropType,
  BiaFieldsConfig,
  BiaFieldNumberFormat,
  NumberMode,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Validators } from '@angular/forms';

// TODO after creation of CRUD Plane : adapt the model
export class Plane extends BaseDto {
  /// BIAToolKit - Begin Properties
  msn: string;
  isActive: boolean;
  lastFlightDate: Date;
  deliveryDate: Date;
  syncTime: string;
  capacity: number;
  probability: number;
  fuelLevel: number;
  estimatedPrice: number;
  siteId: number;
  connectingAirports: OptionDto[];
  planeType: OptionDto | null;
  /// BIAToolKit - End Properties

  DisplayItemName(): string {
    /// BIAToolKit - Begin Display
    return this.msn;
    /// BIAToolKit - End Display
  }
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const PlaneFieldsConfiguration: BiaFieldsConfig = {
  columns: [
    /// BIAToolKit - Begin Block msn
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
      validators: [Validators.required, Validators.maxLength(64)],
    }),
    /// BIAToolKit - End Block msn
    /// BIAToolKit - Begin Block isActive
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isSearchable: true,
      isSortable: false,
      type: PropType.Boolean,
    }),
    /// BIAToolKit - End Block isActive
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
    /// BIAToolKit - Begin Block syncTime
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
    }),
    /// BIAToolKit - End Block syncTime
    /// BIAToolKit - Begin Block capacity
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
      validators: [Validators.required, Validators.min(1)],
    }),
    /// BIAToolKit - End Block capacity
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
    /// BIAToolKit - Begin Block connectingAirports
    Object.assign(
      new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'),
      {
        type: PropType.ManyToMany,
      }
    ),
    /// BIAToolKit - End Block connectingAirports
  ],
};
