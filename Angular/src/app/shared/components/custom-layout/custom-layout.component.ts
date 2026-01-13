import { APP_BASE_HREF } from '@angular/common';
import { Component, Inject } from '@angular/core';
import {
  AuthService,
  BiaTranslationService,
  NavigationService,
} from '@bia-team/bia-ng/core';
import {
  BiaLayoutService,
  BiaUltimaLayoutComponent,
  LayoutComponent,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-custom-layout',
  templateUrl: './custom-layout.component.html',
  imports: [SpinnerComponent, BiaUltimaLayoutComponent],
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
