export interface AppConfig {
  colorScheme: ColorScheme;
  menuMode: MenuMode;
  scale: number;
  showAvatar: boolean;
  alwaysShowInitials: boolean;
  footerMode: FooterMode;
  menuProfilePosition: MenuProfilePosition;
}

export type MenuMode =
  | 'static'
  | 'overlay'
  | 'horizontal'
  | 'slim'
  | 'slim-plus'
  | 'reveal'
  | 'drawer';

export type FooterMode = 'bottom' | 'overlay';

export type ColorScheme = 'light' | 'dark';

export type MenuProfilePosition = 'start' | 'end';
