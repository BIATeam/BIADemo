import { ViewSite } from './view-site';

export interface View {
  id: number;
  tableId: string;
  name: string;
  description: string;
  viewType: number;
  isUserDefault: boolean;
  preference: string;
  viewSites: ViewSite[];
}
