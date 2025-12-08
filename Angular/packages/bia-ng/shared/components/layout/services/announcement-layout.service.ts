import { inject, Injectable, Injector } from '@angular/core';
import {
  AbstractDas,
  BiaAppConstantsService,
  BiaOnlineOfflineService,
  BiaSignalRService,
} from 'packages/bia-ng/core/public-api';
import { Announcement } from 'packages/bia-ng/features/public-api';
import { TargetedFeature } from 'packages/bia-ng/models/public-api';
import { BehaviorSubject, Subscription, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementLayoutService extends AbstractDas<Announcement> {
  private signalRService: BiaSignalRService = inject(BiaSignalRService);
  private biaOnlineOfflineService: BiaOnlineOfflineService = inject(
    BiaOnlineOfflineService
  );

  private targetedFeature: TargetedFeature;
  private pollingInterval: NodeJS.Timeout | undefined;
  private serverAvailable: boolean;
  private sub = new Subscription();
  private firstRefresh: boolean = true;

  activeAnnouncements$: BehaviorSubject<Announcement[]> = new BehaviorSubject<
    Announcement[]
  >([]);

  constructor() {
    super(inject(Injector), 'Announcements');
    this.targetedFeature = {
      parentKey: '',
      featureName: 'announcements',
    };
  }

  init() {
    if (!BiaAppConstantsService.allEnvironments.enableAnnouncements) {
      return;
    }

    // Online/Offline handler
    this.sub.add(
      this.biaOnlineOfflineService.serverAvailable$.subscribe(
        serverAvailable => {
          this.serverAvailable = serverAvailable;
          // Handle first refresh when server available
          if (this.serverAvailable === true && this.firstRefresh) {
            this.refreshActiveAnnouncements();
          }
        }
      )
    );

    // SignalR handler
    this.signalRService.joinGroup(this.targetedFeature);
    this.registerSignalRChanges(() => {
      this.refreshActiveAnnouncements();
    });

    // Polling
    this.pollingInterval = setInterval(() => {
      this.refreshActiveAnnouncements();
    }, 60000);
  }

  destroy() {
    // Polling
    if (this.pollingInterval) {
      clearInterval(this.pollingInterval);
    }

    // SignalR
    this.unregisterSignalRChanges();
    this.signalRService.leaveGroup(this.targetedFeature);

    // Subs
    this.sub?.unsubscribe();
  }

  private refreshActiveAnnouncements() {
    if (this.serverAvailable) {
      this.getListItems<Announcement>({ endpoint: 'actives' })
        .pipe(
          tap(messages => {
            this.activeAnnouncements$.next(messages);
            this.firstRefresh = false;
          })
        )
        .subscribe();
    }
  }

  private registerSignalRChanges(callback: (args: any) => void) {
    console.log(
      '%c [Announcements] Register SignalR : change-announcements',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('change-announcements', args => {
      callback(args);
    });
  }

  private unregisterSignalRChanges() {
    console.log(
      '%c [Announcements] Unregister SignalR : change-announcements',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('change-announcements');
  }
}
