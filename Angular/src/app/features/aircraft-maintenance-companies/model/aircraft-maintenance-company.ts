import { Validators } from '@angular/forms';
import {
  BaseTeamDto,
  teamFieldsConfigurationColumns,
} from 'src/app/shared/bia-shared/model/base-team-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BiaFormLayoutConfig } from 'src/app/shared/bia-shared/model/bia-form-layout-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the model
export class AircraftMaintenanceCompany extends BaseTeamDto {
  admins: OptionDto[];
}

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the field configuration
export const aircraftMaintenanceCompanyFieldsConfiguration: BiaFieldsConfig<AircraftMaintenanceCompany> =
  {
    columns: [
      ...teamFieldsConfigurationColumns,
      ...[
        Object.assign(
          new BiaFieldConfig('admins', 'aircraftMaintenanceCompany.admins'),
          {
            type: PropType.ManyToMany,
            isRequired: true,
            validators: [Validators.required],
            // Begin BIAToolKit Generation Ignore
            isEditable: false,
            // End BIAToolKit Generation Ignore
          }
        ),
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
