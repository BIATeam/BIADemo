import {
  BiaFieldConfig,
  BiaFieldsConfig,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFormLayoutConfig } from 'src/app/shared/bia-shared/model/bia-form-layout-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import {
  TeamDto,
  teamFieldsConfigurationColumns,
} from 'src/app/shared/bia-shared/model/dto/team-dto';
import { VersionedDto } from 'src/app/shared/bia-shared/model/dto/versioned-dto';

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the model
export interface AircraftMaintenanceCompany
  extends BaseDto,
    VersionedDto,
    TeamDto {}

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
            isVisible: false,
            isHideByDefault: true,
            isVisibleInTable: false,
          }
        ),
      ],
    ],
  };

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the form layout configuration
export const aircraftMaintenanceCompanyFormLayoutConfiguration: BiaFormLayoutConfig<AircraftMaintenanceCompany> =
  new BiaFormLayoutConfig([]);
