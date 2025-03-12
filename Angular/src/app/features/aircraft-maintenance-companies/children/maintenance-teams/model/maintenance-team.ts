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
  /// BIAToolKit - Begin Properties
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
  /// BIAToolKit - End Properties
}

// TODO after creation of CRUD Team MaintenanceTeam : adapt the field configuration
export const maintenanceTeamFieldsConfiguration: BiaFieldsConfig<MaintenanceTeam> =
  {
    columns: [
      ...teamFieldsConfigurationColumns,
      ...[
        /// BIAToolKit - Begin Block code
        Object.assign(new BiaFieldConfig('code', 'maintenanceTeam.code'), {
          type: PropType.String,
        }),
        /// BIAToolKit - End Block code
        /// BIAToolKit - Begin Block isActive
        Object.assign(
          new BiaFieldConfig('isActive', 'maintenanceTeam.isActive'),
          {
            type: PropType.Boolean,
          }
        ),
        /// BIAToolKit - End Block isActive
        /// BIAToolKit - Begin Block isApproved
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
        /// BIAToolKit - End Block isApproved
        /// BIAToolKit - Begin Block firstOperation
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
        /// BIAToolKit - End Block firstOperation
        /// BIAToolKit - Begin Block lastOperation
        Object.assign(
          new BiaFieldConfig('lastOperation', 'maintenanceTeam.lastOperation'),
          {
            type: PropType.DateTime,
          }
        ),
        /// BIAToolKit - End Block lastOperation
        /// BIAToolKit - Begin Block approvedDate
        Object.assign(
          new BiaFieldConfig('approvedDate', 'maintenanceTeam.approvedDate'),
          {
            type: PropType.Date,
          }
        ),
        /// BIAToolKit - End Block approvedDate
        /// BIAToolKit - Begin Block nextOperation
        Object.assign(
          new BiaFieldConfig('nextOperation', 'maintenanceTeam.nextOperation'),
          {
            isRequired: true,
            type: PropType.Date,
            validators: [Validators.required],
          }
        ),
        /// BIAToolKit - End Block nextOperation
        /// BIAToolKit - Begin Block maxTravelDuration
        Object.assign(
          new BiaFieldConfig(
            'maxTravelDuration',
            'maintenanceTeam.maxTravelDuration'
          ),
          {
            type: PropType.TimeSecOnly,
          }
        ),
        /// BIAToolKit - End Block maxTravelDuration
        /// BIAToolKit - Begin Block maxOperationDuration
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
        /// BIAToolKit - End Block maxOperationDuration
        /// BIAToolKit - Begin Block operationCount
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
        /// BIAToolKit - End Block operationCount
        /// BIAToolKit - Begin Block incidentCount
        Object.assign(
          new BiaFieldConfig('incidentCount', 'maintenanceTeam.incidentCount'),
          {
            type: PropType.Number,
            filterMode: PrimeNGFiltering.Equals,
          }
        ),
        /// BIAToolKit - End Block incidentCount
        /// BIAToolKit - Begin Block totalOperationDuration
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
        /// BIAToolKit - End Block totalOperationDuration
        /// BIAToolKit - Begin Block averageOperationDuration
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
        /// BIAToolKit - End Block averageOperationDuration
        /// BIAToolKit - Begin Block totalTravelDuration
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
        /// BIAToolKit - End Block totalTravelDuration
        /// BIAToolKit - Begin Block averageTravelDuration
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
        /// BIAToolKit - End Block averageTravelDuration
        /// BIAToolKit - Begin Block totalOperationCost
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
        /// BIAToolKit - End Block totalOperationCost
        /// BIAToolKit - Begin Block averageOperationCost
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
        /// BIAToolKit - End Block averageOperationCost
        /// BIAToolKit - Begin Block currentCountry
        Object.assign(
          new BiaFieldConfig(
            'currentCountry',
            'maintenanceTeam.currentCountry'
          ),
          {
            type: PropType.OneToMany,
          }
        ),
        /// BIAToolKit - End Block currentCountry
        /// BIAToolKit - Begin Block operationCountries
        Object.assign(
          new BiaFieldConfig(
            'operationCountries',
            'maintenanceTeam.operationCountries'
          ),
          {
            type: PropType.ManyToMany,
          }
        ),
        /// BIAToolKit - End Block operationCountries
        /// BIAToolKit - Begin Block currentAirport
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
        /// BIAToolKit - End Block currentAirport
        /// BIAToolKit - Begin Block operationAirports
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
        /// BIAToolKit - End Block operationAirports
        Object.assign(
          new BiaFieldConfig('rowVersion', 'maintenanceTeam.rowVersion'),
          {
            isVisible: false,
            isHideByDefault: true,
          }
        ),
      ],
    ],
  };

// TODO after creation of CRUD Team MaintenanceTeam : adapt the form layout configuration
export const maintenanceTeamFormLayoutConfiguration: BiaFormLayoutConfig<MaintenanceTeam> =
  new BiaFormLayoutConfig([]);
