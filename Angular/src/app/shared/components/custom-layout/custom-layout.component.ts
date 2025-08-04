import { APP_BASE_HREF, NgIf } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  AuthService,
  BiaTranslationService,
  NavigationService,
} from 'bia-ng/core';
import {
  BiaLayoutService,
  BiaUltimaLayoutComponent,
  LayoutComponent,
  SpinnerComponent,
} from 'bia-ng/shared';

@Component({
  selector: 'app-custom-layout',
  templateUrl: './custom-layout.component.html',
  imports: [NgIf, SpinnerComponent, BiaUltimaLayoutComponent],
})
export class CustomLayoutComponent extends LayoutComponent {
  constructor(
    public biaTranslationService: BiaTranslationService,
    protected navigationService: NavigationService,
    protected authService: AuthService,
    protected readonly layoutService: BiaLayoutService,
    protected readonly store: Store,
    // protected notificationSignalRService: NotificationSignalRService,
    @Inject(APP_BASE_HREF) public baseHref: string
  ) {
    super(
      biaTranslationService,
      navigationService,
      authService,
      layoutService,
      store,
      baseHref
    );
  }
}
