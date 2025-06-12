import {
  BiaFieldConfig,
  BiaFieldsConfig,
  PropType,
} from 'src/app/shared/bia-shared/model/bia-field-config';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export interface NotificationListItem extends BaseDto {
  titleTranslated: string;
  descriptionTranslated: string;
  type: OptionDto;
  read: boolean;
  createdDate: string;
  createdBy: OptionDto | null;
  siteId: number;
  notifiedUsers: OptionDto[];
  notifiedTeams: OptionDto[];
  jData: string;
}

export const notificationFieldsConfiguration: BiaFieldsConfig<NotificationListItem> =
  {
    columns: [
      new BiaFieldConfig('titleTranslated', 'notification.title'),
      new BiaFieldConfig('descriptionTranslated', 'notification.description'),
      Object.assign(new BiaFieldConfig('type', 'notification.type.title'), {
        type: PropType.OneToMany,
      }),
      Object.assign(new BiaFieldConfig('read', 'notification.read'), {
        type: PropType.Boolean,
      }),
      Object.assign(
        new BiaFieldConfig('createdDate', 'notification.createdDate'),
        {
          type: PropType.DateTime,
        }
      ),
      Object.assign(new BiaFieldConfig('createdBy', 'notification.createdBy'), {
        type: PropType.OneToMany,
      }),
      Object.assign(
        new BiaFieldConfig('notifiedUsers', 'notification.notifiedUsers'),
        {
          type: PropType.ManyToMany,
        }
      ),
      Object.assign(
        new BiaFieldConfig('notifiedTeams', 'notification.notifiedTeams'),
        {
          type: PropType.ManyToMany,
        }
      ),
      new BiaFieldConfig('jData', 'notification.jData'),
    ],
  };
