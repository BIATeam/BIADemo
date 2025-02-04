import { BaseTeamDto } from 'src/app/shared/bia-shared/model/base-team-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the model
export type AircraftMaintenanceCompany = BaseTeamDto;

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the field configuration
export const aircraftMaintenanceCompanyFieldsConfiguration: BiaFieldsConfig<AircraftMaintenanceCompany> =
  {
    columns: [
      Object.assign(new BiaFieldConfig('title', 'site.title'), {
        isRequired: true,
      }),
      Object.assign(
        new BiaFieldConfig('admins', 'aircraftMaintenanceCompany.admins'),
        {
          isEditable: false,
          type: PropType.ManyToMany,
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
        }
      ),
    ],
  };
