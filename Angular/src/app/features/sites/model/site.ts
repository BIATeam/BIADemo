import { BiaFieldConfig, BiaFieldsConfig, PropType } from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

// TODO after CRUD creation : adapt the model
export interface Site extends BaseDto {
  title: string;
}


// TODO after CRUD creation : adapt the field configuration
export const SiteFieldsConfiguration : BiaFieldsConfig =
{
  columns: [
    Object.assign(new BiaFieldConfig('title', 'site.title'), {
      isRequired: true,
    }),
    Object.assign(new BiaFieldConfig('siteAdmins', 'site.admins'), {
      isEditable: false,
      type: PropType.ManyToMany,
    }),
  ]
}