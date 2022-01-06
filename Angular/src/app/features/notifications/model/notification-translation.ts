import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

export interface NotificationTranslation extends BaseDto {
  languageId: number;
  title: string;
  description: string;
}
