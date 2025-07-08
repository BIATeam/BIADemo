import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { TeamDto } from 'src/app/shared/bia-shared/model/dto/team-dto';
import { VersionedDto } from 'src/app/shared/bia-shared/model/dto/versioned-dto';

// TODO after creation of CRUD Team Site : adapt the model
export interface Site extends BaseDto, TeamDto, VersionedDto {}

// TODO after creation of CRUD Team Site : adapt the field configuration
export const siteFieldsConfiguration: BiaFieldsConfig<Site> = {
  columns: [
    Object.assign(new BiaFieldConfig<Site>('title', 'site.title'), {
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig<Site>('admins', 'site.admins'), {
      isEditable: false,
      isVisible: false,
      type: PropType.ManyToMany,
      filterWithDisplay: false,
    }),
    Object.assign(new BiaFieldConfig('rowVersion', 'site.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};
