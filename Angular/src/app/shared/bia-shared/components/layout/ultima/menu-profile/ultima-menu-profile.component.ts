import { animate, style, transition, trigger } from '@angular/animations';
import { Component, ElementRef, Input, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaThemeService } from 'src/app/core/bia-core/services/bia-theme.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { THEME_DARK, THEME_LIGHT } from 'src/app/shared/constants';
import { BiaLayoutService } from '../../services/layout.service';

@Component({
  selector: 'bia-ultima-menu-profile',
  templateUrl: './ultima-menu-profile.component.html',
  styleUrls: ['./ultima-menu-profile.component.scss'],
  animations: [
    trigger('menu', [
      transition('void => inline', [
        style({ height: 0 }),
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 1, height: '*' })
        ),
      ]),
      transition('inline => void', [
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 0, height: '0' })
        ),
      ]),
      transition('void => overlay', [
        style({ opacity: 0, transform: 'scaleY(0.8)' }),
        animate('.12s cubic-bezier(0, 0, 0.2, 1)'),
      ]),
      transition('overlay => void', [
        animate('.1s linear', style({ opacity: 0 })),
      ]),
    ]),
  ],
})
export class BiaUltimaMenuProfileComponent implements OnDestroy {
  @Input()
  set username(name: string | undefined) {
    if (name) {
      this.usernameParam = { name };
    }
    this.buildTopBarMenu();
  }
  @Input() supportedLangs: string[];
  @Input() set login(value: string) {
    const url =
      this.appSettingsService.appSettings.profileConfiguration?.urlProfileImage.replace(
        '{login}',
        value
      ) ?? '';
    this.avatarUrl = url;
  }

  avatarUrl = 'assets/bia/img/PersonPlaceholder.png';

  usernameParam?: { name: string };
  displayName: string;
  topBarMenuItems: MenuItem[];

  private sub: Subscription = new Subscription();

  constructor(
    private readonly translateService: TranslateService,
    protected readonly layoutService: BiaLayoutService,
    protected biaTranslation: BiaTranslationService,
    protected biaTheme: BiaThemeService,
    protected readonly appSettingsService: AppSettingsService,
    public el: ElementRef
  ) {}

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  toggleMenu() {
    this.layoutService.onMenuProfileToggle();
  }

  get isHorizontal() {
    return this.layoutService.isHorizontal() && this.layoutService.isDesktop();
  }

  get menuProfileActive(): boolean {
    return this.layoutService.state.menuProfileActive;
  }
  set menuProfileActive(_: boolean) {
    this.layoutService.onMenuProfileToggle();
  }

  get isTooltipDisabled(): boolean {
    return !this.layoutService.isSlim();
  }

  protected onChangeTheme(theme: string) {
    this.biaTheme.changeTheme(theme);
  }

  protected onChangeLanguage(lang: string) {
    this.biaTranslation.loadAndChangeLanguage(lang);
  }

  buildTopBarMenu() {
    const translationKeys = [
      'bia.lang.fr',
      'bia.lang.de',
      'bia.lang.es',
      'bia.lang.gb',
      'bia.lang.mx',
      'bia.lang.us',
      'bia.greetings',
      'bia.languages',
      'bia.theme',
      'bia.themeLight',
      'bia.themeDark',
    ];

    this.sub.add(
      this.translateService.stream(translationKeys).subscribe(translations => {
        const menuItemLang: MenuItem[] = [];

        if (this.supportedLangs) {
          this.supportedLangs.forEach(lang => {
            menuItemLang.push({
              label:
                translations['bia.lang.' + lang.split('-')[1].toLowerCase()],
              command: () => {
                this.onChangeLanguage(lang);
              },
            });
          });
        }

        this.displayName =
          translations['bia.greetings'] +
          ' ' +
          (this.usernameParam?.name ?? '');

        this.topBarMenuItems = [
          {
            label: translations['bia.languages'],
            items: menuItemLang,
          },
          {
            label: translations['bia.theme'],
            items: [
              {
                label: translations['bia.themeLight'],
                command: () => {
                  this.onChangeTheme(THEME_LIGHT);
                },
              },
              {
                label: translations['bia.themeDark'],
                command: () => {
                  this.onChangeTheme(THEME_DARK);
                },
              },
            ],
          },
        ];
      })
    );
  }

  onImgError() {
    this.avatarUrl = 'assets/bia/img/PersonPlaceholder.png';
  }
}