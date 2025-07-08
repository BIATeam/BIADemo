import { animate, style, transition, trigger } from '@angular/animations';
import { CommonModule, NgIf, NgTemplateOutlet } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import {
  Component,
  effect,
  ElementRef,
  HostBinding,
  Input,
  OnDestroy,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModule } from 'primeng/api';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { Dialog } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { Tooltip } from 'primeng/tooltip';
import { catchError, map, Observable, Subscription, take, tap } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaEnvironmentService } from 'src/app/core/bia-core/services/bia-environment.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';
import { Permission } from 'src/app/shared/permission';
import { BiaLayoutService } from '../../services/layout.service';
import { BiaMenuProfileService } from '../../services/menu-profile.service';

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
  imports: [
    Tooltip,
    NgIf,
    Dialog,
    SharedModule,
    TranslateModule,
    NgTemplateOutlet,
    ButtonModule,
    InputTextModule,
    FormsModule,
    CommonModule,
    AvatarModule,
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
      this.usernameParam.name = name;
    }
    this.buildTopBarMenu();
  }

  @Input()
  set lastname(lastname: string | undefined) {
    this.usernameParam.lastname = lastname ?? '';
  }

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

  protected readonly defaultProfileImage =
    'assets/bia/img/PersonPlaceholder.png';

  externalImage = false;
  avatarUrl: string | SafeUrl = this.defaultProfileImage;

  usernameParam: { name: string; lastname: string } = {
    name: '',
    lastname: '',
  };
  get initials(): string {
    const { name, lastname } = this.usernameParam;
    const nameInitial = name.length ? name[0] : '';
    const lastnameInitial = lastname.length ? lastname[0] : '';

    return nameInitial + lastnameInitial;
  }

  menuProfileHtml: string;

  protected sub: Subscription = new Subscription();

  urlEditAvatar: string;
  signInAs: string;
  permissions = Permission;

  constructor(
    protected readonly layoutService: BiaLayoutService,
    protected biaTranslation: BiaTranslationService,
    protected readonly appSettingsService: AppSettingsService,
    protected readonly http: HttpClient,
    protected readonly sanitizer: DomSanitizer,
    public el: ElementRef,
    protected readonly menuProfileService: BiaMenuProfileService,
    protected readonly authService: AuthService
  ) {
    this.urlEditAvatar =
      this.appSettingsService.appSettings.profileConfiguration
        ?.editProfileImageUrl ?? '';

    this.sub.add(
      this.biaTranslation.currentCulture$.subscribe(() => {
        this.buildTopBarMenu();
      })
    );

    effect(() => {
      this.buildTopBarMenu();
    });
  }

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

  buildTopBarMenu() {
    this.menuProfileHtml = this.menuProfileService.getMenuProfileHtml(
      this.usernameParam?.name ?? ''
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

  connectWithSpecificRights() {
    this.authService.setLoginParameters({
      ...this.authService.getLoginParameters(),
      baseUserIdentity: this.signInAs,
    });
    location.reload();
  }
}
