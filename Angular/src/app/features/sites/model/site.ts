import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseTeamDto } from 'src/app/shared/bia-shared/model/base-team-dto';

// TODO after creation of CRUD Team Site : adapt the model
export type Site = BaseTeamDto;

// TODO after creation of CRUD Team Site : adapt the field configuration
export const siteFieldsConfiguration: BiaFieldsConfig = {
  columns: [
    Object.assign(new BiaFieldConfig('title', 'site.title'), {
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('admins', 'site.admins'), {
      isEditable: false,
      isVisible: false,
      type: PropType.ManyToMany,
    }),
  ],
};
