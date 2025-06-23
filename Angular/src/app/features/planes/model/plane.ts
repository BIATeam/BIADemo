import { Validators } from '@angular/forms';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
  BiaFieldNumberFormat,
  BiaFieldsConfig,
  NumberMode,
  PrimeNGFiltering,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import {
  BiaFormLayoutConfig,
  BiaFormLayoutConfigColumnSize,
  BiaFormLayoutConfigField,
  BiaFormLayoutConfigGroup,
  BiaFormLayoutConfigRow,
} from 'src/app/shared/bia-shared/model/bia-form-layout-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { FixableDto } from 'src/app/shared/bia-shared/model/dto/fixable-dto';
import { VersionedDto } from 'src/app/shared/bia-shared/model/dto/versioned-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD Plane : adapt the model
export interface Plane extends BaseDto, VersionedDto, FixableDto {
  siteId: number;
  msn: string;
  manufacturer: string | null;
  isActive: boolean;
  isMaintenance: boolean | null;
  firstFlightDate: Date;
  lastFlightDate: Date | null;
  deliveryDate: Date | null;
  nextMaintenanceDate: Date;
  syncTime: string;
  syncFlightDataTime: string | null;
  capacity: number;
  motorsCount: number | null;
  totalFlightHours: number;
  probability: number | null;
  fuelCapacity: number;
  fuelLevel: number | null;
  originalPrice: number;
  estimatedPrice: number | null;
  planeType: OptionDto | null;
  similarTypes: OptionDto[] | null;
  currentAirport: OptionDto;
  connectingAirports: OptionDto[];
}

// TODO after creation of CRUD Plane : adapt the field configuration
export const planeFieldsConfiguration: BiaFieldsConfig<Plane> = {
  columns: [
    Object.assign(new BiaFieldConfig('msn', 'plane.msn'), {
      type: PropType.String,
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('manufacturer', 'plane.manufacturer'), {
      type: PropType.String,
    }),
    Object.assign(new BiaFieldConfig('isActive', 'plane.isActive'), {
      type: PropType.Boolean,
      isRequired: true,
      // Begin BIAToolKit Generation Ignore
      isSearchable: true,
      isSortable: false,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('isMaintenance', 'plane.isMaintenance'), {
      type: PropType.Boolean,
      // Begin BIAToolKit Generation Ignore
      isSearchable: true,
      isSortable: false,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(
      new BiaFieldConfig('firstFlightDate', 'plane.firstFlightDate'),
      {
        type: PropType.DateTime,
        isRequired: true,
      }
    ),
    Object.assign(
      new BiaFieldConfig('lastFlightDate', 'plane.lastFlightDate'),
      {
        type: PropType.DateTime,
        // Begin BIAToolKit Generation Ignore
        displayFormat: Object.assign(new BiaFieldDateFormat(), {
          autoFormatDate: 'yyyy',
          autoPrimeDateFormat: 'yyyy',
        }),
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(new BiaFieldConfig('deliveryDate', 'plane.deliveryDate'), {
      type: PropType.Date,
    }),
    Object.assign(
      new BiaFieldConfig('nextMaintenanceDate', 'plane.nextMaintenanceDate'),
      {
        type: PropType.Date,
        isRequired: true,
      }
    ),
    Object.assign(new BiaFieldConfig('syncTime', 'plane.syncTime'), {
      type: PropType.TimeSecOnly,
      isRequired: true,
    }),
    Object.assign(
      new BiaFieldConfig('syncFlightDataTime', 'plane.syncFlightDataTime'),
      {
        type: PropType.TimeSecOnly,
      }
    ),
    Object.assign(new BiaFieldConfig('capacity', 'plane.capacity'), {
      type: PropType.Number,
      isRequired: true,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('motorsCount', 'plane.motorsCount'), {
      type: PropType.Number,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(
      new BiaFieldConfig('totalFlightHours', 'plane.totalFlightHours'),
      {
        type: PropType.Number,
        isRequired: true,
        // Begin BIAToolKit Generation Ignore
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Decimal,
          minFractionDigits: 6,
          maxFractionDigits: 6,
        }),
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(new BiaFieldConfig('probability', 'plane.probability'), {
      type: PropType.Number,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 6,
        maxFractionDigits: 6,
      }),
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('fuelCapacity', 'plane.fuelCapacity'), {
      type: PropType.Number,
      isRequired: true,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 2,
        maxFractionDigits: 2,
      }),
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('fuelLevel', 'plane.fuelLevel'), {
      type: PropType.Number,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Decimal,
        minFractionDigits: 2,
        maxFractionDigits: 2,
      }),
      validators: [Validators.min(0), Validators.max(100)],
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(new BiaFieldConfig('originalPrice', 'plane.originalPrice'), {
      type: PropType.Number,
      isRequired: true,
      // Begin BIAToolKit Generation Ignore
      filterMode: PrimeNGFiltering.Equals,
      displayFormat: Object.assign(new BiaFieldNumberFormat(), {
        mode: NumberMode.Currency,
        minFractionDigits: 2,
        maxFractionDigits: 2,
        currency: 'EUR',
      }),
      // End BIAToolKit Generation Ignore
    }),
    Object.assign(
      new BiaFieldConfig('estimatedPrice', 'plane.estimatedPrice'),
      {
        type: PropType.Number,
        // Begin BIAToolKit Generation Ignore
        filterMode: PrimeNGFiltering.Equals,
        displayFormat: Object.assign(new BiaFieldNumberFormat(), {
          mode: NumberMode.Currency,
          minFractionDigits: 2,
          maxFractionDigits: 2,
          currency: 'EUR',
        }),
        validators: [Validators.min(0)],
        // End BIAToolKit Generation Ignore
      }
    ),
    Object.assign(new BiaFieldConfig('planeType', 'plane.planeType'), {
      type: PropType.OneToMany,
    }),
    Object.assign(new BiaFieldConfig('similarTypes', 'plane.similarTypes'), {
      type: PropType.ManyToMany,
    }),
    Object.assign(
      new BiaFieldConfig('currentAirport', 'plane.currentAirport'),
      {
        type: PropType.OneToMany,
        isRequired: true,
      }
    ),
    Object.assign(
      new BiaFieldConfig('connectingAirports', 'plane.connectingAirports'),
      {
        type: PropType.ManyToMany,
        isRequired: true,
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'plane.rowVersion'), {
      isVisible: false,
      isHideByDefault: true,
      isVisibleInTable: false,
    }),
  ],
};

// TODO after creation of CRUD Plane : adapt the form layout configuration
export const planeFormLayoutConfiguration: BiaFormLayoutConfig<Plane> =
  new BiaFormLayoutConfig([
    // Begin BIAToolKit Generation Ignore
    new BiaFormLayoutConfigRow([
      new BiaFormLayoutConfigGroup('plane.groupIdentification', [
        new BiaFormLayoutConfigRow([
          new BiaFormLayoutConfigField('msn'),
          new BiaFormLayoutConfigField('manufacturer'),
        ]),
      ]),
      new BiaFormLayoutConfigGroup('plane.groupStatus', [
        new BiaFormLayoutConfigRow([
          new BiaFormLayoutConfigField('isActive'),
          new BiaFormLayoutConfigField('isMaintenance'),
        ]),
      ]),
    ]),
    new BiaFormLayoutConfigGroup('plane.groupTracking', [
      new BiaFormLayoutConfigRow([
        new BiaFormLayoutConfigField('deliveryDate'),
        new BiaFormLayoutConfigField('firstFlightDate'),
        new BiaFormLayoutConfigField('lastFlightDate'),
        new BiaFormLayoutConfigField('nextMaintenanceDate'),
      ]),
      new BiaFormLayoutConfigRow([
        new BiaFormLayoutConfigField('syncFlightDataTime'),
        new BiaFormLayoutConfigField('syncTime'),
      ]),
    ]),
    new BiaFormLayoutConfigRow([
      new BiaFormLayoutConfigField(
        'motorsCount',
        new BiaFormLayoutConfigColumnSize(6, 6, 6, 6)
      ),
    ]),
    new BiaFormLayoutConfigRow([
      new BiaFormLayoutConfigField('probability'),
      new BiaFormLayoutConfigField('capacity'),
    ]),
    // End BIAToolKit Generation Ignore
  ]);
