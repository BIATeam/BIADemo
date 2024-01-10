import { PrimeNGFiltering, BiaFieldConfig, PropType, BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Validators } from '@angular/forms';

// TODO after creation of CRUD Plane : adapt the model
export interface Plane extends BaseDto {
  /// BIAToolKit - Begin properties
  msn: string;
  isActive: boolean;
  lastFlightDate: Date;
  deliveryDate: Date;
  syncTime: string;
  capacity: number;
  siteId: number;
  connectingAirports: OptionDto[];
  planeType: OptionDto | null;
  /// BIAToolKit - End properties
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const PlaneFieldsConfiguration: BiaFieldsConfig =
{
  columns: [
    /// BIAToolKit - Begin block msn
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
    })
    /// BIAToolKit - End block msn
    ,
    /// BIAToolKit - Begin block isActive
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isSearchable: true,
      isSortable: false,
      type: PropType.Boolean,
    })
    /// BIAToolKit - End block isActive
    ,
    /// BIAToolKit - Begin block lastFlightDate
    Object.assign(new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'), {
      type: PropType.DateTime,
    })
    /// BIAToolKit - End block lastFlightDate
    ,
    /// BIAToolKit - Begin block deliveryDate
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
    })
    /// BIAToolKit - End block deliveryDate
    ,
    /// BIAToolKit - Begin block syncTime
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
    })
    /// BIAToolKit - End block syncTime
    ,
    /// BIAToolKit - Begin block capacity
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
      validators: [Validators.required, Validators.min(1)],
    })
    /// BIAToolKit - End block capacity
    ,
    /// BIAToolKit - Begin block planeType
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
    })
    /// BIAToolKit - End block planeType
    ,
    /// BIAToolKit - Begin block connectingAirports
    Object.assign(new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'), {
      type: PropType.ManyToMany,
    })
    /// BIAToolKit - End block connectingAirports
  ]
}