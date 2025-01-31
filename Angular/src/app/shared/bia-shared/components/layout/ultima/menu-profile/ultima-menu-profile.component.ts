import { animate, style, transition, trigger } from '@angular/animations';
import { HttpClient } from '@angular/common/http';
import {
  Component,
  ElementRef,
  HostBinding,
  Input,
  OnDestroy,
} from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';
import { Observable, Subscription, catchError, map, take, tap } from 'rxjs';
import { BiaEnvironmentService } from 'src/app/core/bia-core/services/bia-environment.service';
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
        // eslint-disable-next-line @typescript-eslint/naming-convention
        style({ height: 0, 'overflow-y': 'hidden' }),
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 1, height: '*' })
        ),
      ]),
      transition('inline => void', [
        // eslint-disable-next-line @typescript-eslint/naming-convention
        style({ 'overflow-y': 'hidden' }),
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 0, height: '0' })
        ),
      ]),
      transition('void => overlay', [
        animate('.12s cubic-bezier(0, 0, 0.2, 1)'),
      ]),
      transition('overlay => void', [
        animate('.1s linear', style({ opacity: 0 })),
      ]),
    ]),
  ],
})
export class BiaUltimaMenuProfileComponent implements OnDestroy {
  @HostBinding('class.layout-menu-profile-no-fill') get noFill() {
    return (
      !this.layoutService.isHorizontal() ||
      this.layoutService.state.isSmallScreen
    );
  }

  @Input()
  set username(name: string | undefined) {
    if (name) {
      this.usernameParam = { name };
    }
    this.buildTopBarMenu();
  }
  @Input() supportedLangs: string[];
  @Input() set login(value: string) {
    const profile = this.appSettingsService.appSettings.profileConfiguration;
    this.externalImage = profile?.clientProfileImageGetMode ?? false;
    if (profile?.profileImageUrlOrPath) {
      if (!this.externalImage) {
        this.avatarUrl = this.defaultProfileImage;
        this.getImage(BiaEnvironmentService.getApiUrl() + '/ProfileImage/get')
          .pipe(
            take(1),
            tap(value => (this.avatarUrl = value)),
            catchError(() => (this.avatarUrl = this.defaultProfileImage))
          )
          .subscribe();
      } else {
        this.avatarUrl = profile?.profileImageUrlOrPath.replace(
          '{login}',
          value
        );
      }
    } else {
      this.avatarUrl = this.defaultProfileImage;
    }
  }

  private readonly defaultProfileImage = 'assets/bia/img/PersonPlaceholder.png';

  externalImage = false;
  avatarUrl: string | SafeUrl = this.defaultProfileImage;

  usernameParam?: { name: string };
  displayName: string;
  topBarMenuItems: MenuItem[];

  private sub: Subscription = new Subscription();

  constructor(
    protected readonly translateService: TranslateService,
    protected readonly layoutService: BiaLayoutService,
    protected biaTranslation: BiaTranslationService,
    protected biaTheme: BiaThemeService,
    protected readonly appSettingsService: AppSettingsService,
    protected readonly http: HttpClient,
    protected readonly sanitizer: DomSanitizer,
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
      'bia.language',
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

          menuItemLang.sort((a, b) => {
            const labelA = a.label || '';
            const labelB = b.label || '';
            return labelA.localeCompare(labelB);
          });
        }

        this.displayName =
          translations['bia.greetings'] +
          ' ' +
          (this.usernameParam?.name ?? '');

        this.topBarMenuItems = [
          {
            label: translations['bia.language'],
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
    this.avatarUrl = this.defaultProfileImage;
  }

  getImage(url: string): Observable<SafeUrl> {
    return this.http
      .get(url, { responseType: 'blob' })
      .pipe(
        map(val =>
          this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(val))
        )
      );
  }
}
