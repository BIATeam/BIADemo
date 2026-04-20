import { AsyncPipe, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateModule } from '@ngx-translate/core';
import {
  AuthService,
  BiaFileDownloaderService,
} from 'packages/bia-ng/core/public-api';
import { NotificationDetailComponent } from 'packages/bia-ng/features/notifications/views/notification-detail/notification-detail.component';
import { NotificationService } from 'packages/bia-ng/features/public-api';
import {
  NotificationTeamWarningComponent,
  SpinnerComponent,
} from 'packages/bia-ng/shared/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { ButtonDirective } from 'primeng/button';
import { Observable } from 'rxjs';
import { SpecificNotification } from '../../model/specific-notification';
import { SpecificNotificationService } from '../../services/specific-notification.service';

@Component({
  selector: 'app-specific-notification-detail',
  templateUrl: './specific-notification-detail.component.html',
  styleUrls: ['./specific-notification-detail.component.scss'],
  providers: [
    { provide: NotificationService, useClass: SpecificNotificationService },
    { provide: SpecificNotificationService, useExisting: NotificationService },
  ],
  imports: [
    ButtonDirective,
    NotificationTeamWarningComponent,
    AsyncPipe,
    DatePipe,
    TranslateModule,
    SpinnerComponent,
  ],
})
export class SpecificNotificationDetailComponent extends NotificationDetailComponent {
  constructor(
    protected override store: Store<BiaAppState>,
    protected override router: Router,
    protected override activatedRoute: ActivatedRoute,
    protected override authService: AuthService,
    public override notificationService: SpecificNotificationService,
    protected override fileDownloaderService: BiaFileDownloaderService
  ) {
    super(
      store,
      router,
      activatedRoute,
      authService,
      notificationService,
      fileDownloaderService
    );
  }

  get specificNotification$(): Observable<SpecificNotification | undefined> {
    return this.notification$ as Observable<SpecificNotification | undefined>;
  }

  onAcknowledge(notification: SpecificNotification) {
    this.onSubmitted({
      ...notification,
      acknowledgedAt: new Date(),
    } as SpecificNotification);
  }
}
