import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { FieldValidator } from '../../../validators/field.validator';

// TODO after creation of CRUD Member : adapt the model
export interface Member extends BaseDto {
  user: OptionDto;
  lastName: string;
  firstName: string;
  login: string;
  isActive: boolean;
  roles: OptionDto[];
  teamId: number;
}

export class Members {
  users: OptionDto[];
  roles: OptionDto[];
  teamId: number;
}

// TODO after creation of CRUD Member : adapt the field configuration
export const memberFieldsConfiguration: BiaFieldsConfig<Member> = {
  formValidators: [FieldValidator.atLeastOneFilled(['user', 'login'])],
  columns: [
    Object.assign(new BiaFieldConfig<Member>('user', 'member.user'), {
      //isRequired: true,
      type: PropType.OneToMany,
    }),
    Object.assign(new BiaFieldConfig('lastName', 'user.lastName'), {
      isEditable: false,
      isHideByDefault: true,
    }),
    Object.assign(new BiaFieldConfig('firstName', 'user.firstName'), {
      isEditable: false,
      isHideByDefault: true,
    }),
    Object.assign(new BiaFieldConfig('login', 'user.login'), {
      isEditable: false,
      isOnlyInitializable: true,
    }),
    Object.assign(new BiaFieldConfig('isActive', 'member.isActive'), {
      isEditable: false,
      type: PropType.Boolean,
      isHideByDefault: true,
    }),
    Object.assign(new BiaFieldConfig<Member>('roles', 'member.roles'), {
      type: PropType.ManyToMany,
    }),
  ],
};
