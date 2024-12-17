import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';

// TODO after creation of CRUD PlaneType : adapt the model
export interface PlaneType extends BaseDto {
  title: string;
  certificationDate: Date;
}

// TODO after creation of CRUD PlaneType : adapt the field configuration
export const planeTypeFieldsConfiguration: BiaFieldsConfig<PlaneType> = {
  columns: [
    Object.assign(new BiaFieldConfig('title', 'planeType.title'), {
      isRequired: true,
    }),
    Object.assign(
      new BiaFieldConfig('certificationDate', 'planeType.certificationDate'),
      {
        type: PropType.Date,
        isRequired: true,
      }
    ),
  ],
};
