import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

// TODO after creation of CRUD User : adapt the model
export interface User extends BaseDto {
  lastName: string;
  firstName: string;
  login: string;
  guid: string;
  // Computed by Angular when data receive
  displayName: string;
  roles: OptionDto[];
}

// TODO after creation of CRUD User : adapt the field configuration
export const userFieldsConfiguration: BiaFieldsConfig<User> = {
  columns: [
    Object.assign(new BiaFieldConfig('lastName', 'user.lastName'), {
      isEditable: false,
    }),
    Object.assign(new BiaFieldConfig('firstName', 'user.firstName'), {
      isEditable: false,
    }),
    Object.assign(new BiaFieldConfig('login', 'user.login'), {
      isEditable: false,
      isOnlyInitializable: true,
    }),
    Object.assign(new BiaFieldConfig('roles', 'member.roles'), {
      type: PropType.ManyToMany,
    }),
  ],
};
