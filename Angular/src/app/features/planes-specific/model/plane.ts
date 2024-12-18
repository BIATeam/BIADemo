import { Validators } from '@angular/forms';
import { PrimeIcons } from 'primeng/api';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PrimeNGFiltering,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD Plane : adapt the model
export interface Plane extends BaseDto {
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
        validators: [Validators.required],
        minWidth: '50px',
      }
    ),
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      filterMode: PrimeNGFiltering.Equals,
      specificOutput: true,
      specificInput: true,
      isRequired: true,
      validators: [Validators.required, Validators.min(1)],
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
        validators: [Validators.required],
        minWidth: '50px',
      }
    ),
    Object.assign(
      new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'),
      {
        type: PropType.ManyToMany,
        minWidth: '50px',
      }
    ),
  ],
};
