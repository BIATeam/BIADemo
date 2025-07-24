import { Validators } from '@angular/forms';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  OptionDto,
  VersionedDto,
} from 'biang/models';
import { PrimeNGFiltering, PropType } from 'biang/models/enum';
import { PrimeIcons } from 'primeng/api';

// TODO after creation of CRUD Plane : adapt the model
export interface Plane extends BaseDto, VersionedDto {
  msn: string;
  isActive: boolean;
  lastFlightDate: Date;
  deliveryDate: Date;
  syncFlightDataTime: string;
  capacity: number;
  siteId: number;
  planeType: OptionDto | null;
  connectingAirports: OptionDto[];
  currentAirport: OptionDto;
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const planeFieldsConfiguration: BiaFieldsConfig<Plane> = {
  columns: [
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      isRequired: true,
      isFrozen: true,
      minWidth: '50px',
      maxConstraints: 5,
    }),
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      isSearchable: false,
      specificOutput: true,
      specificInput: true,
      type: PropType.Boolean,
      icon: PrimeIcons.POWER_OFF,
      minWidth: '50px',
    }),
    Object.assign(
      new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'),
      {
        type: PropType.DateTime,
        minWidth: '50px',
      }
    ),
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
      minWidth: '50px',
    }),
    Object.assign(
      new BiaFieldConfig('syncFlightDataTime', 'plane.syncFlightDataTime'),
      {
        isRequired: true,
        type: PropType.TimeSecOnly,
        minWidth: '50px',
        isHideByDefault: true,
      }
    ),
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      specificOutput: true,
      specificInput: true,
      isRequired: true,
      validators: [Validators.min(1)],
      minWidth: '50px',
    }),
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
      minWidth: '50px',
    }),
    Object.assign(
      new BiaFieldConfig('currentAirport', 'plane.currentAirport'),
      {
        isRequired: true,
        type: PropType.OneToMany,
        minWidth: '50px',
      }
    ),
    Object.assign(
      new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'),
      {
        type: PropType.ManyToMany,
        minWidth: '50px',
        specificInput: true,
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'plane.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};
