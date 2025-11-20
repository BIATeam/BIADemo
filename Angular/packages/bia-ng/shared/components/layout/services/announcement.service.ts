import { Injectable, Injector } from '@angular/core';
import {
  AbstractDas,
  BiaSignalRService,
} from 'packages/bia-ng/core/public-api';
import { Announcement } from 'packages/bia-ng/features/public-api';
import { TargetedFeature } from 'packages/bia-ng/models/public-api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementService extends AbstractDas<Announcement> {
  private targetedFeature: TargetedFeature;
  private signalRService: BiaSignalRService;

  constructor(protected injector: Injector) {
    super(injector, 'Announcements');

    this.signalRService =
      this.injector.get<BiaSignalRService>(BiaSignalRService);
    this.targetedFeature = {
      parentKey: '',
      featureName: 'announcements',
    };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  getActives(): Observable<Announcement[]> {
    return this.getListItems<Announcement>({ endpoint: 'actives' });
  }

  registerSignalRChanges(callback: (args: any) => void) {
    console.log(
      '%c [Announcements] Register SignalR : change-announcements',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('change-announcements', args => {
      callback(args);
    });
  }

  destroy() {
    this.unregisterChanges();
    this.signalRService.leaveGroup(this.targetedFeature);
  }

  private unregisterChanges() {
    console.log(
      '%c [Announcements] Unregister SignalR : change-announcements',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('change-announcements');
  }
}
