import { AsyncPipe, NgIf } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { BiaTranslationService } from 'biang/core';
import { BiaLayoutService, SpinnerComponent } from 'biang/shared';
import { BiaAppState } from 'biang/store';
import { Observable, Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { Notification } from '../../model/notification';
import { NotificationService } from '../../services/notification.service';
import { FeatureNotificationsStore } from '../../store/notification.state';

@Component({
  selector: 'bia-notifications-item',
  templateUrl: './notification-item.component.html',
  styleUrls: ['./notification-item.component.scss'],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
})
export class NotificationItemComponent implements OnInit, OnDestroy {
  notification$: Observable<Notification>;
  protected sub = new Subscription();

  constructor(
    protected store: Store<BiaAppState>,
    protected route: ActivatedRoute,
    public notificationService: NotificationService,
    protected layoutService: BiaLayoutService,
    protected biaTranslationService: BiaTranslationService
  ) {}

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.notificationService.currentCrudItemId =
          this.route.snapshot.params.notificationId;
      })
    );

    this.sub.add(
      this.store
        .select(FeatureNotificationsStore.getCurrentNotification)
        .subscribe(notification => {
          if (notification?.title) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] = notification.title;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
