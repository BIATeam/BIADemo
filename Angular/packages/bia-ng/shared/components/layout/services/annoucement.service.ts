import { Injectable, Injector } from '@angular/core';
import {
  AbstractDas,
  BiaSignalRService,
} from 'packages/bia-ng/core/public-api';
import { Annoucement } from 'packages/bia-ng/features/public-api';
import { TargetedFeature } from 'packages/bia-ng/models/public-api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AnnoucementService extends AbstractDas<Annoucement> {
  private targetedFeature: TargetedFeature;
  private signalRService: BiaSignalRService;

  constructor(protected injector: Injector) {
    super(injector, 'Annoucements');

    this.signalRService =
      this.injector.get<BiaSignalRService>(BiaSignalRService);
    this.targetedFeature = {
      parentKey: '',
      featureName: 'annoucements',
    };
    this.signalRService.joinGroup(this.targetedFeature);
  }

  getActives(): Observable<Annoucement[]> {
    return this.getListItems<Annoucement>({ endpoint: 'actives' });
  }

  registerSignalRChanges(callback: (args: any) => void) {
    console.log(
      '%c [Annoucements] Register SignalR : change-annoucements',
      'color: purple; font-weight: bold'
    );
    this.signalRService.addMethod('change-annoucements', args => {
      callback(args);
    });
  }

  destroy() {
    this.unregisterChanges();
    this.signalRService.leaveGroup(this.targetedFeature);
  }

  private unregisterChanges() {
    console.log(
      '%c [Annoucements] Unregister SignalR : change-annoucements',
      'color: purple; font-weight: bold'
    );
    this.signalRService.removeMethod('change-annoucements');
  }
}
