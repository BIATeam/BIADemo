import {
  BiaBannerType,
  PropType,
} from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  BiaFieldsConfig,
  BiaFormLayoutConfig,
  VersionedDto,
} from 'packages/bia-ng/models/public-api';

// TODO after creation of CRUD BannerMessage : adapt the model
export interface BannerMessage extends BaseDto, VersionedDto {
  end: Date;
  name: string;
  rawContent: string;
  start: Date;
  type: BiaBannerType;
}

// TODO after creation of CRUD BannerMessage : adapt the field configuration
export const bannerMessageFieldsConfiguration: BiaFieldsConfig<BannerMessage> =
  {
    columns: [
      Object.assign(new BiaFieldConfig('end', 'bannerMessage.end'), {
        type: PropType.DateTime,
        isRequired: true,
      }),
      Object.assign(new BiaFieldConfig('name', 'bannerMessage.name'), {
        type: PropType.String,
        isRequired: true,
      }),
      Object.assign(
        new BiaFieldConfig('rawContent', 'bannerMessage.rawContent'),
        {
          type: PropType.String,
          isRequired: true,
        }
      ),
      Object.assign(new BiaFieldConfig('start', 'bannerMessage.start'), {
        type: PropType.DateTime,
        isRequired: true,
      }),
      Object.assign(new BiaFieldConfig('type', 'bannerMessage.type'), {
        isRequired: true,
      }),
      Object.assign(
        new BiaFieldConfig('rowVersion', 'bannerMessage.rowVersion'),
        {
          isVisible: false,
          isVisibleInTable: false,
        }
      ),
    ],
  };

// TODO after creation of CRUD BannerMessage : adapt the form layout configuration
export const bannerMessageFormLayoutConfiguration: BiaFormLayoutConfig<BannerMessage> =
  new BiaFormLayoutConfig([]);
