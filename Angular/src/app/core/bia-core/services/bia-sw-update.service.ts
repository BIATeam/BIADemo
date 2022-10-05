import { Injectable } from '@angular/core';
import { SwUpdate, VersionReadyEvent } from '@angular/service-worker';
import { filter, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class BiaSwUpdateService {

  private _newVersionAvailable: boolean;
  public get newVersionAvailable(): boolean {
    return this._newVersionAvailable;
  }

  constructor(protected swUpdate: SwUpdate) {
    this.init();
  }

  public init() {
    if (this.swUpdate?.isEnabled === true) {
      const updatesAvailable$ = this.getUpdatesAvailable();
      updatesAvailable$.subscribe(async () => {
        this._newVersionAvailable = true;
      });
    }
  }

  public async checkForUpdate() {
    if (this.swUpdate?.isEnabled === true) {
      await this.swUpdate.checkForUpdate();
    }
  }

  public async activateUpdate() {
    if (this.swUpdate?.isEnabled === true) {
      await this.swUpdate.activateUpdate();
    }
  }

  public getUpdatesAvailable() {
    return this.swUpdate.versionUpdates.pipe(
      filter((evt): evt is VersionReadyEvent => evt.type === 'VERSION_READY'),
      map(evt => ({
        type: 'UPDATE_AVAILABLE',
        current: evt.currentVersion,
        available: evt.latestVersion,
      })));
  }

}