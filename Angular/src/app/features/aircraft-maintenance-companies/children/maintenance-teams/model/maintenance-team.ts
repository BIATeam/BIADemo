import { BiaFieldConfig, BiaFieldsConfig } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

// TODO after creation of CRUD Team MaintenanceTeam : adapt the model
export interface MaintenanceTeam extends BaseDto {
  title: string;
}

// TODO after creation of CRUD Team MaintenanceTeam : adapt the field configuration
export const MaintenanceTeamFieldsConfiguration : BiaFieldsConfig =
{
  columns: [
    Object.assign(new BiaFieldConfig('title', 'site.title'), {
      isRequired: true,
    }),
  ]
}