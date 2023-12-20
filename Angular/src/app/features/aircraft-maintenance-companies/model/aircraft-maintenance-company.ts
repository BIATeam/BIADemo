import { BiaFieldConfig, BiaFieldsConfig, PropType } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseTeamDto } from 'src/app/shared/bia-shared/model/base-team-dto';

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the model
export interface AircraftMaintenanceCompany extends BaseTeamDto {
}

// TODO after creation of CRUD Team AircraftMaintenanceCompany : adapt the field configuration
export const AircraftMaintenanceCompanyFieldsConfiguration : BiaFieldsConfig =
{
  columns: [
    Object.assign(new BiaFieldConfig('title', 'site.title'), {
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('admins', 'aircraftMaintenanceCompany.admins'), {
      isEditable: false,
      type: PropType.ManyToMany,
    }),
  ]
}