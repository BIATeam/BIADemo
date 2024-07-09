import {
  BiaFieldConfig,
  BiaFieldsConfig,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseTeamDto } from 'src/app/shared/bia-shared/model/base-team-dto';

// TODO after creation of CRUD Team MaintenanceTeam : adapt the model
export type MaintenanceTeam = BaseTeamDto;

// TODO after creation of CRUD Team MaintenanceTeam : adapt the field configuration
export const maintenanceTeamFieldsConfiguration: BiaFieldsConfig = {
  columns: [
    Object.assign(new BiaFieldConfig('title', 'site.title'), {
      isRequired: true,
    }),
  ],
};
