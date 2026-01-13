import { MenuMode } from './layout/app-config';

export interface ConfigDisplay {
  showEditAvatar: boolean;
  showLang: boolean;
  showScale: boolean;
  showTheme: boolean;
  showMenuStyle: MenuMode[];
  showFooterStyle: boolean;
  showMenuProfilePosition: boolean;
}
