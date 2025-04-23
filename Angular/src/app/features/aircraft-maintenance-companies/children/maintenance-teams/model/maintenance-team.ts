import { Validators } from '@angular/forms';
import {
  BaseTeamDto,
  teamFieldsConfigurationColumns,
} from 'src/app/shared/bia-shared/model/base-team-dto';
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

// TODO after creation of CRUD Team MaintenanceTeam : adapt the model
export class MaintenanceTeam extends BaseTeamDto {
  code: string;
  isActive: boolean;
  isApproved: boolean | null;
  firstOperation: Date;
  lastOperation: Date | null;
  approvedDate: Date | null;
  nextOperation: Date;
  maxTravelDuration: string;
  maxOperationDuration: string;
  operationCount: number;
  incidentCount: number | null;
  totalOperationDuration: number;
  averageOperationDuration: number | null;
  totalTravelDuration: number;
  averageTravelDuration: number | null;
  totalOperationCost: number;
  averageOperationCost: number | null;
  currentCountry: OptionDto | null;
  operationCountries: OptionDto[];
  operationAirports: OptionDto[];
  currentAirport: OptionDto;
}

// TODO after creation of CRUD Team MaintenanceTeam : adapt the field configuration
export const maintenanceTeamFieldsConfiguration: BiaFieldsConfig<MaintenanceTeam> =
  {
    columns: [
      ...teamFieldsConfigurationColumns,
      ...[
        Object.assign(new BiaFieldConfig('code', 'maintenanceTeam.code'), {
          type: PropType.String,
        }),
        Object.assign(
          new BiaFieldConfig('isActive', 'maintenanceTeam.isActive'),
          {
            type: PropType.Boolean,
          }
        ),
        Object.assign(
          new BiaFieldConfig('isApproved', 'maintenanceTeam.isApproved'),
          {
            isRequired: true,
            isSearchable: true,
            isSortable: false,
            type: PropType.Boolean,
            validators: [Validators.required],
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'firstOperation',
            'maintenanceTeam.firstOperation'
          ),
          {
            isRequired: true,
            type: PropType.DateTime,
            validators: [Validators.required],
          }
        ),
        Object.assign(
          new BiaFieldConfig('lastOperation', 'maintenanceTeam.lastOperation'),
          {
            type: PropType.DateTime,
          }
        ),
        Object.assign(
          new BiaFieldConfig('approvedDate', 'maintenanceTeam.approvedDate'),
          {
            type: PropType.Date,
          }
        ),
        Object.assign(
          new BiaFieldConfig('nextOperation', 'maintenanceTeam.nextOperation'),
          {
            isRequired: true,
            type: PropType.Date,
            validators: [Validators.required],
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'maxTravelDuration',
            'maintenanceTeam.maxTravelDuration'
          ),
          {
            type: PropType.TimeSecOnly,
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'maxOperationDuration',
            'maintenanceTeam.maxOperationDuration'
          ),
          {
            isRequired: true,
            type: PropType.TimeSecOnly,
            validators: [Validators.required],
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'operationCount',
            'maintenanceTeam.operationCount'
          ),
          {
            type: PropType.Number,
            filterMode: PrimeNGFiltering.Equals,
            isRequired: true,
            validators: [Validators.required, Validators.min(1)],
          }
        ),
        Object.assign(
          new BiaFieldConfig('incidentCount', 'maintenanceTeam.incidentCount'),
          {
            type: PropType.Number,
            filterMode: PrimeNGFiltering.Equals,
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'totalOperationDuration',
            'maintenanceTeam.totalOperationDuration'
          ),
          {
            isRequired: true,
            type: PropType.Number,
            filterMode: PrimeNGFiltering.Equals,
            displayFormat: Object.assign(new BiaFieldNumberFormat(), {
              mode: NumberMode.Decimal,
              minFractionDigits: 6,
              maxFractionDigits: 6,
            }),
            validators: [Validators.required, Validators.min(0)],
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'averageOperationDuration',
            'maintenanceTeam.averageOperationDuration'
          ),
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
          new BiaFieldConfig(
            'totalTravelDuration',
            'maintenanceTeam.totalTravelDuration'
          ),
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
        Object.assign(
          new BiaFieldConfig(
            'averageTravelDuration',
            'maintenanceTeam.averageTravelDuration'
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
        Object.assign(
          new BiaFieldConfig(
            'totalOperationCost',
            'maintenanceTeam.totalOperationCost'
          ),
          {
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
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'averageOperationCost',
            'maintenanceTeam.averageOperationCost'
          ),
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
          new BiaFieldConfig(
            'currentCountry',
            'maintenanceTeam.currentCountry'
          ),
          {
            type: PropType.OneToMany,
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'operationCountries',
            'maintenanceTeam.operationCountries'
          ),
          {
            type: PropType.ManyToMany,
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'currentAirport',
            'maintenanceTeam.currentAirport'
          ),
          {
            isRequired: true,
            type: PropType.OneToMany,
            validators: [Validators.required],
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'operationAirports',
            'maintenanceTeam.operationAirports'
          ),
          {
            isRequired: true,
            type: PropType.ManyToMany,
            validators: [Validators.required],
          }
        ),
        Object.assign(
          new BiaFieldConfig('rowVersion', 'maintenanceTeam.rowVersion'),
          {
            isVisible: false,
            isHideByDefault: true,
            isVisibleInTable: false,
          }
        ),
      ],
    ],
  };

// TODO after creation of CRUD Team MaintenanceTeam : adapt the form layout configuration
export const maintenanceTeamFormLayoutConfiguration: BiaFormLayoutConfig<MaintenanceTeam> =
  new BiaFormLayoutConfig([]);
