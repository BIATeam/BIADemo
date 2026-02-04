import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  TeamDto,
  VersionedDto,
} from '@bia-team/bia-ng/models';
import { PropType } from '@bia-team/bia-ng/models/enum';

// TODO after creation of CRUD Team Site : adapt the model
export interface Site extends BaseDto, TeamDto, VersionedDto {
  // Begin BIADemo
  uniqueIdentifier: string;
  // End BIADemo
}

// TODO after creation of CRUD Team Site : adapt the field configuration
export const siteFieldsConfiguration: BiaFieldsConfig<Site> = {
  columns: [
    Object.assign(new BiaFieldConfig<Site>('title', 'site.title'), {
      isRequired: true,
    }),
    // Begin BIADemo
    Object.assign(
      new BiaFieldConfig<Site>('uniqueIdentifier', 'site.uniqueIdentifier'),
      {}
    ),
    // End BIADemo
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
