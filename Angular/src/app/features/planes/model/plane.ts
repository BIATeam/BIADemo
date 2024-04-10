import { PrimeNGFiltering, BiaFieldConfig, PropType, BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
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
export const PlaneFieldsConfiguration: BiaFieldsConfig =
{
  columns: [
    /// BIAToolKit - Begin Block msn
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
    })
    /// BIAToolKit - End Block msn
    ,
    /// BIAToolKit - Begin Block isActive
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isSearchable: true,
      isSortable: false,
      type: PropType.Boolean,
    })
    /// BIAToolKit - End Block isActive
    ,
    /// BIAToolKit - Begin Block lastFlightDate
    Object.assign(new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'), {
      type: PropType.DateTime,
    })
    /// BIAToolKit - End Block lastFlightDate
    ,
    /// BIAToolKit - Begin Block deliveryDate
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
    })
    /// BIAToolKit - End Block deliveryDate
    ,
    /// BIAToolKit - Begin Block syncTime
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
    })
    /// BIAToolKit - End Block syncTime
    ,
    /// BIAToolKit - Begin Block capacity
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
      validators: [Validators.required, Validators.min(1)],
    })
    /// BIAToolKit - End Block capacity
    ,
    /// BIAToolKit - Begin Block planeType
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
    })
    /// BIAToolKit - End Block planeType
    ,
    /// BIAToolKit - Begin Block connectingAirports
    Object.assign(new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'), {
      type: PropType.ManyToMany,
    })
    /// BIAToolKit - End Block connectingAirports
  ]
}