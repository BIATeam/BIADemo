import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  VersionedDto,
} from 'biang/models';
import { PropType } from 'biang/models/enum';

// TODO after creation of CRUD PlaneType : adapt the model
export interface PlaneType extends BaseDto, VersionedDto {
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
    Object.assign(new BiaFieldConfig('rowVersion', 'planeType.rowVersion'), {
      isVisible: false,
      isVisibleInTable: false,
    }),
  ],
};
