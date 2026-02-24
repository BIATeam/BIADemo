import { Injectable } from '@angular/core';
import { TargetedFeature } from 'packages/bia-ng/models/target-feature';
import { BiaSignalRService } from './bia-signalr.service';

@Injectable({ providedIn: 'root' })
export class BiaDataChangeService {
  protected targetedFeature: TargetedFeature;
  protected entityReloadFlags: Record<string, boolean> = {};
  protected methodName: string = 'refresh-entity';
  protected isInitialized: boolean = false;

  constructor(protected biaSignalRService: BiaSignalRService) {
    this.targetedFeature = {
      parentKey: '',
      featureName: 'change-entity',
    };
  }

  public initialize(): void {
    if (this.isInitialized === true) {
      return;
    }
    this.isInitialized = true;

    this.biaSignalRService.joinGroup(this.targetedFeature);

    console.log(
      '%c [' +
        this.targetedFeature.featureName +
        '] Register SignalR : ' +
        this.methodName,
      'color: purple; font-weight: bold'
    );

    this.biaSignalRService.addMethod(this.methodName, args => {
      const storeKey: string = JSON.parse(args);
      console.debug(`Received change notification for storeKey: ${storeKey}`);
      this.entityReloadFlags[storeKey] = true;
    });

    this.biaSignalRService.getHubConnection().onreconnected(() => {
      this.clearFlags();
    });
  }

  public needsReload(entity: string): boolean {
    if (!(entity in this.entityReloadFlags)) {
      this.entityReloadFlags[entity] = false;
      return true;
    }
    console.debug(
      `StoreKey: ${entity} - Reload Flag: ${this.entityReloadFlags[entity]}`
    );
    return !!this.entityReloadFlags[entity];
  }

  public markReloaded(entity: string): void {
    this.entityReloadFlags[entity] = false;
    console.debug(
      `StoreKey: ${entity} - Reload Flag: ${this.entityReloadFlags[entity]}`
    );
  }

  public destroy(): void {
    this.biaSignalRService.removeMethod(this.methodName);
    this.biaSignalRService.leaveGroup(this.targetedFeature);
    this.isInitialized = false;
  }

  protected clearFlags(): void {
    console.debug('Clearing all reload flags due to SignalR connection loss');
    this.entityReloadFlags = {};
  }
}
