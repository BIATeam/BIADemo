import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { AppState } from 'src/app/store/state';
import { Notification } from '../../model/notification';
import { NotificationOptionsService } from '../../services/notification-options.service';
import { NotificationService } from '../../services/notification.service';
import { FeatureNotificationsActions } from '../../store/notifications-actions';

@Component({
  selector: 'bia-notification-edit',
  templateUrl: './notification-edit.component.html',
  styleUrls: ['./notification-edit.component.scss'],
})
export class NotificationEditComponent implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  protected sub = new Subscription();

  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    public notificationOptionsService: NotificationOptionsService,
    public notificationService: NotificationService,
    protected biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.notificationOptionsService.loadAllOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(notificationToUpdate: Notification) {
    this.store.dispatch(
      FeatureNotificationsActions.update({ notification: notificationToUpdate })
    );
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
