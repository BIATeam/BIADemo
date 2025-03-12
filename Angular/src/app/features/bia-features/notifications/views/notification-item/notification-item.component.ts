import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { AppState } from 'src/app/store/state';
import { Notification } from '../../model/notification';
import { NotificationService } from '../../services/notification.service';
import { FeatureNotificationsStore } from '../../store/notification.state';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'bia-notifications-item',
    templateUrl: './notification-item.component.html',
    styleUrls: ['./notification-item.component.scss'],
    imports: [RouterOutlet, NgIf, BiaSharedModule, AsyncPipe]
})
export class NotificationItemComponent implements OnInit, OnDestroy {
  notification$: Observable<Notification>;
  protected sub = new Subscription();

  constructor(
    protected store: Store<AppState>,
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
