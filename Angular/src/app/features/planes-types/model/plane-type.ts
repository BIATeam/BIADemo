import {
  PropType,
  TableColumnVisibility,
} from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  VersionedDto,
} from 'packages/bia-ng/models/public-api';

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
      maxlength: 64,
    }),
    Object.assign(
      new BiaFieldConfig('certificationDate', 'planeType.certificationDate'),
      {
        type: PropType.Date,
        isRequired: true,
      }
    ),
    Object.assign(new BiaFieldConfig('rowVersion', 'planeType.rowVersion'), {
      isVisibleInForm: false,
      tableColumnVisibility: TableColumnVisibility.Hidden,
    }),
  ],
};
