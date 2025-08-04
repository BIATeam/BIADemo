import { Validators } from '@angular/forms';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldNumberFormat,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  FixableDto,
  OptionDto,
  TeamDto,
  teamFieldsConfigurationColumns,
  VersionedDto,
} from 'bia-ng/models';
import { NumberMode, PrimeNGFiltering, PropType } from 'bia-ng/models/enum';

// TODO after creation of CRUD Team MaintenanceTeam : adapt the model
export interface MaintenanceTeam
  extends BaseDto,
    VersionedDto,
    TeamDto,
    FixableDto {
  code: string | null;
  isActive: boolean;
  isApproved: boolean | null;
  firstOperation: Date;
  lastOperation: Date | null;
  approvedDate: Date | null;
  nextOperation: Date;
  maxTravelDuration: string | null;
  maxOperationDuration: string;
  operationCount: number;
  incidentCount: number | null;
  totalOperationDuration: number;
  averageOperationDuration: number | null;
  totalTravelDuration: number;
  averageTravelDuration: number | null;
  totalOperationCost: number;
  averageOperationCost: number | null;
  aircraftMaintenanceCompanyId: number;
  currentAirport: OptionDto;
  operationAirports: OptionDto[];
  currentCountry: OptionDto | null;
  operationCountries: OptionDto[] | null;
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
            isRequired: true,
          }
        ),
        Object.assign(
          new BiaFieldConfig('isApproved', 'maintenanceTeam.isApproved'),
          {
            type: PropType.Boolean,
            // Begin BIAToolKit Generation Ignore
            isSearchable: true,
            isSortable: false,
            // End BIAToolKit Generation Ignore
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'firstOperation',
            'maintenanceTeam.firstOperation'
          ),
          {
            type: PropType.DateTime,
            isRequired: true,
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
            type: PropType.Date,
            isRequired: true,
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
            type: PropType.TimeSecOnly,
            isRequired: true,
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'operationCount',
            'maintenanceTeam.operationCount'
          ),
          {
            type: PropType.Number,
            isRequired: true,
            // Begin BIAToolKit Generation Ignore
            filterMode: PrimeNGFiltering.Equals,
            // End BIAToolKit Generation Ignore
          }
        ),
        Object.assign(
          new BiaFieldConfig('incidentCount', 'maintenanceTeam.incidentCount'),
          {
            type: PropType.Number,
            // Begin BIAToolKit Generation Ignore
            filterMode: PrimeNGFiltering.Equals,
            // End BIAToolKit Generation Ignore
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'totalOperationDuration',
            'maintenanceTeam.totalOperationDuration'
          ),
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
        Object.assign(
          new BiaFieldConfig(
            'averageOperationDuration',
            'maintenanceTeam.averageOperationDuration'
          ),
          {
            type: PropType.Number,
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
        Object.assign(
          new BiaFieldConfig(
            'totalTravelDuration',
            'maintenanceTeam.totalTravelDuration'
          ),
          {
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
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'averageTravelDuration',
            'maintenanceTeam.averageTravelDuration'
          ),
          {
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
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'totalOperationCost',
            'maintenanceTeam.totalOperationCost'
          ),
          {
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
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'averageOperationCost',
            'maintenanceTeam.averageOperationCost'
          ),
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
        Object.assign(
          new BiaFieldConfig(
            'currentAirport',
            'maintenanceTeam.currentAirport'
          ),
          {
            type: PropType.OneToMany,
            isRequired: true,
          }
        ),
        Object.assign(
          new BiaFieldConfig(
            'operationAirports',
            'maintenanceTeam.operationAirports'
          ),
          {
            type: PropType.ManyToMany,
            isRequired: true,
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
          new BiaFieldConfig('rowVersion', 'maintenanceTeam.rowVersion'),
          {
            isVisible: false,
            isVisibleInTable: false,
          }
        ),
      ],
    ],
  };

// TODO after creation of CRUD Team MaintenanceTeam : adapt the form layout configuration
export const maintenanceTeamFormLayoutConfiguration: BiaFormLayoutConfig<MaintenanceTeam> =
  new BiaFormLayoutConfig([]);
