import { APP_BASE_HREF, NgIf } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { NavigationService } from 'src/app/core/bia-core/services/navigation.service';
import { LayoutComponent } from '../../bia-shared/components/layout/layout.component';
import { BiaLayoutService } from '../../bia-shared/components/layout/services/layout.service';
import { BiaUltimaLayoutComponent } from '../../bia-shared/components/layout/ultima/layout/ultima-layout.component';
import { SpinnerComponent } from '../../bia-shared/components/spinner/spinner.component';

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
