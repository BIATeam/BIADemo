import { SiteMember } from './site-member';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';

export interface SiteInfo extends BaseDto {
  title: string;
  siteAdmins: SiteMember[];
}
