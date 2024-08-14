import {
  Injectable,
  Renderer2,
  RendererFactory2,
  Inject,
  OnDestroy,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { THEME_LIGHT, THEME_DARK } from 'src/app/shared/constants';

export const STORAGE_THEME_KEY = 'theme';
const DEFAULT_THEME = THEME_LIGHT;

@Injectable({
  providedIn: 'root',
})
export class BiaThemeService implements OnDestroy {
  private renderer: Renderer2;
  private document: Document;
  private currentTheme = new BehaviorSubject<string | undefined>(undefined);
  private browserDarkThemeQuery = window.matchMedia(
    '(prefers-color-scheme: dark)'
  );
  private browserLightThemeQuery = window.matchMedia(
    '(prefers-color-scheme: light)'
  );
  private browserThemeChangeListener: () => void;

  isCurrentThemeDark$ = this.currentTheme.pipe(
    map(currentTheme => currentTheme && /dark/.test(currentTheme))
  );

  constructor(
    rendererFactory: RendererFactory2,
    @Inject(DOCUMENT) document: any
  ) {
    this.renderer = rendererFactory.createRenderer(null, null);
    this.document = document;
    this.changeTheme(localStorage.getItem(STORAGE_THEME_KEY) || DEFAULT_THEME);

    this.enableBrowserThemeChangeDetection(theme => {
      console.log(theme);
    });
  }

  ngOnDestroy(): void {
    this.stopBrowserThemeChangeListener();
  }

  getThemeSelected(): string | null {
    return localStorage.getItem(STORAGE_THEME_KEY);
  }

  // TODO More sophisticated theme system maybe?
  changeTheme(theme: string) {
    if (theme !== null) {
      theme = theme.toLowerCase();
    }

    if (theme !== this.currentTheme.value) {
      this.applyTheme(theme, this.currentTheme.value);
    }

    try {
      localStorage.setItem(STORAGE_THEME_KEY, theme);
    } catch (err) {
      console.error(err);
    }
  }

  applyBrowserTheme() {
    this.changeTheme(this.getBrowserTheme() || DEFAULT_THEME);
  }

  enableBrowserThemeChangeDetection(
    onBrowserThemeChange: (theme: string | undefined) => void
  ) {
    const browserTheme = this.getBrowserTheme();
    if (browserTheme) {
      this.changeTheme(browserTheme);
    }

    this.startBrowserThemeChangeListener(onBrowserThemeChange);
  }

  disableBrowserThemeChangeDetection() {
    this.stopBrowserThemeChangeListener();
  }

  private applyTheme(theme: string, oldTheme?: string) {
    if (oldTheme && oldTheme !== DEFAULT_THEME) {
      this.renderer.removeClass(this.document.body, `${oldTheme}-theme`);
    } else if (theme !== DEFAULT_THEME) {
      this.renderer.addClass(this.document.body, `${theme}-theme`);
    }
    this.applyPrimeNgTheme(theme);
    this.currentTheme.next(theme);
  }

  private applyPrimeNgTheme(theme: string) {
    const themeLightLink: HTMLLinkElement = document.getElementById(
      'theme-light-css'
    ) as HTMLLinkElement;
    //const layoutLightLink: HTMLLinkElement = document.getElementById('layout-light-css') as HTMLLinkElement;

    const themeDarkLink: HTMLLinkElement = document.getElementById(
      'theme-dark-css'
    ) as HTMLLinkElement;
    //const layoutDarkLink: HTMLLinkElement = document.getElementById('layout-dark-css') as HTMLLinkElement;

    themeLightLink.disabled = theme === THEME_DARK;
    //layoutLightLink.disabled = theme === THEME_DARK;

    themeDarkLink.disabled = theme === THEME_LIGHT;
    //layoutDarkLink.disabled = theme === THEME_LIGHT;
  }

  private getBrowserTheme(): string | null {
    if (this.browserDarkThemeQuery.matches) {
      return 'dark';
    }
    if (this.browserLightThemeQuery.matches) {
      return 'light';
    }
    return null;
  }

  private startBrowserThemeChangeListener(
    onBrowserThemeChange: (theme: string | undefined) => void
  ) {
    // Set listener
    this.browserThemeChangeListener = () => {
      this.applyBrowserTheme();
      onBrowserThemeChange(this.currentTheme.value);
    };
    // Subscribe listener to browser theme change event
    this.browserDarkThemeQuery.addEventListener(
      'change',
      this.browserThemeChangeListener
    );
  }

  private stopBrowserThemeChangeListener() {
    this.browserDarkThemeQuery.removeEventListener(
      'change',
      this.browserThemeChangeListener
    );
  }
}
