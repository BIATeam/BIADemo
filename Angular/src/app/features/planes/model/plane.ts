import { PrimeNGFiltering, BiaFieldConfig, PropType, BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after CRUD creation : adapt the model
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

// TODO after CRUD creation : adapt the field configuration
export const PlaneFieldsConfiguration : BiaFieldsConfig =
{
  columns: [
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isSearchable: false,
      isSortable: false,
      type: PropType.Boolean,
    }),
    Object.assign(new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'), {
      type: PropType.DateTime,
    }),
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
    }),
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
    }),
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
    }),
    Object.assign(new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'), {
      type: PropType.ManyToMany,
    })
  ]
}