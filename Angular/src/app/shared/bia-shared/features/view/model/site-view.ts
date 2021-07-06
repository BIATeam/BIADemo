import { View } from './view';
import { Site } from 'src/app/domains/site/model/site';

export interface SiteView extends View {
  siteId: number;
  site: Site;
}
