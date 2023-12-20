import { PrimeNGFiltering, BiaFieldConfig, PropType, BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Validators } from '@angular/forms';
import { PrimeIcons } from 'primeng/api';

// TODO after creation of CRUD Plane : adapt the model
export interface Plane extends BaseDto {
  msn: string;
  isActive: boolean;
  lastFlightDate: Date;
  deliveryDate: Date;
  syncTime: string;
  capacity: number;
  siteId: number;
  connectingAirports: OptionDto[];
  planeType: OptionDto | null;
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const PlaneFieldsConfiguration : BiaFieldsConfig =
{
  columns: [
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
      isFrozen: true,
      minWidth: '50px'
    }),
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isSearchable: false,
      isSortable: false,
      specificOutput: true,
      specificInput:true,
      type: PropType.Boolean,
      icon: PrimeIcons.POWER_OFF,
      minWidth: '50px'
    }),
    Object.assign(new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'), {
      type: PropType.DateTime,
      minWidth: '50px'
    }),
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
      minWidth: '50px'
    }),
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
      minWidth: '50px'
    }),
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      specificOutput: true,
      specificInput:true,
      isRequired: true,
      validators: [Validators.required, Validators.min(1)],
      minWidth: '50px'
    }),
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
      minWidth: '50px'
    }),
    Object.assign(new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'), {
      type: PropType.ManyToMany,
      minWidth: '50px'
    })
  ],
}