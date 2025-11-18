import { Injectable, Injector } from '@angular/core';
import {
  AbstractDas,
  BiaSignalRService,
} from 'packages/bia-ng/core/public-api';
import { BannerMessage } from 'packages/bia-ng/features/public-api';
import { TargetedFeature } from 'packages/bia-ng/models/public-api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BannerService extends AbstractDas<BannerMessage> {
  private targetedFeature: TargetedFeature;
  private signalRService: BiaSignalRService;

  constructor(protected injector: Injector) {
    super(injector, 'BannerMessages');

    this.signalRService =
      this.injector.get<BiaSignalRService>(BiaSignalRService);
    this.targetedFeature = {
      parentKey: '',
      featureName: 'banner-messages',
    };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  getActives(): Observable<BannerMessage[]> {
    return this.getListItems<BannerMessage>({ endpoint: 'actives' });
  }

  registerSignalRChanges(callback: (args: any) => void) {
    console.log(
      '%c [BannerMessages] Register SignalR : change-banner-messages',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('change-banner-messages', args => {
      callback(args);
    });
  }

  destroy() {
    this.unregisterChanges();
    this.signalRService.leaveGroup(this.targetedFeature);
  }

  private unregisterChanges() {
    console.log(
      '%c [BannerMessages] Unregister SignalR : change-banner-messages',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('change-banner-messages');
  }
}
