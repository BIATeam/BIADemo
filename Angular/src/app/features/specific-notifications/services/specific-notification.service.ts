import { Injectable, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthService } from 'packages/bia-ng/core/public-api';
import {
  NotificationDas,
  NotificationOptionsService,
  NotificationService,
  NotificationsSignalRService,
} from 'packages/bia-ng/features/public-api';
import { BiaAppState } from 'packages/bia-ng/store/public-api';
import { specificNotificationCRUDConfiguration } from '../specific-notification.constants';

@Injectable()
export class SpecificNotificationService extends NotificationService {
  public override crudConfiguration = specificNotificationCRUDConfiguration;

  constructor(
    protected override store: Store<BiaAppState>,
    public override dasService: NotificationDas,
    public override signalRService: NotificationsSignalRService,
    public override optionsService: NotificationOptionsService,
    protected override injector: Injector,
    protected override authService: AuthService
  ) {
    super(
      store,
      dasService,
      signalRService,
      optionsService,
      injector,
      authService
    );
  }
}
