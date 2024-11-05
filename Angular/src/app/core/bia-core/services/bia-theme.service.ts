import { DOCUMENT } from '@angular/common';
import { Inject, Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  BiaLayoutService,
  ColorScheme,
} from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { THEME_DARK, THEME_LIGHT } from 'src/app/shared/constants';

export const STORAGE_THEME_KEY = 'theme';
const DEFAULT_THEME = THEME_LIGHT;

@Injectable({
  providedIn: 'root',
})
export class BiaThemeService {
  protected renderer: Renderer2;
  protected document: Document;
  protected currentTheme = new BehaviorSubject<string | undefined>(undefined);

  isCurrentThemeDark$ = this.currentTheme.pipe(
    map(currentTheme => currentTheme && /dark/.test(currentTheme))
  );

  constructor(
    private readonly layoutService: BiaLayoutService,
    rendererFactory: RendererFactory2,
    @Inject(DOCUMENT) document: any
  ) {
    this.renderer = rendererFactory.createRenderer(null, null);
    this.document = document;
    let theme;
    try {
      theme = localStorage.getItem(STORAGE_THEME_KEY) || DEFAULT_THEME;
    } catch {
      theme = DEFAULT_THEME;
    }
    this.applyTheme(theme);
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

  protected applyTheme(theme: string, oldTheme?: string) {
    if (oldTheme && oldTheme !== DEFAULT_THEME) {
      this.renderer.removeClass(this.document.body, `${oldTheme}-theme`);
    } else if (theme !== DEFAULT_THEME) {
      this.renderer.addClass(this.document.body, `${theme}-theme`);
    }
    this.applyPrimeNgTheme(theme);
    this.currentTheme.next(theme);
    this.layoutService.config().colorScheme = theme as ColorScheme;
  }

  protected applyPrimeNgTheme(theme: string) {
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
}
