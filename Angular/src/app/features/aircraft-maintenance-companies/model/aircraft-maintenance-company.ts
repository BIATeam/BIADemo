import { TableColumnVisibility } from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  TeamDto,
  teamFieldsConfigurationColumns,
  VersionedDto,
} from 'packages/bia-ng/models/public-api';

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the model
export interface AircraftMaintenanceCompany
  extends BaseDto, VersionedDto, TeamDto {}

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the field configuration
export const aircraftMaintenanceCompanyFieldsConfiguration: BiaFieldsConfig<AircraftMaintenanceCompany> =
  {
    columns: [
      ...teamFieldsConfigurationColumns,
      ...[
        Object.assign(
          new BiaFieldConfig(
            'rowVersion',
            'aircraftMaintenanceCompany.rowVersion'
          ),
          {
            isVisibleInForm: false,
            tableColumnVisibility: TableColumnVisibility.Hidden,
          }
        ),
      ],
    ],
  };

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the form layout configuration
export const aircraftMaintenanceCompanyFormLayoutConfiguration: BiaFormLayoutConfig<AircraftMaintenanceCompany> =
  new BiaFormLayoutConfig([]);
