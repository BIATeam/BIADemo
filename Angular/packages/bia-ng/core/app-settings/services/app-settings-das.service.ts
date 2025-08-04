import { Injectable, Injector } from '@angular/core';
import { AppSettings } from 'packages/bia-ng/models/public-api';
import { AbstractDas } from '../../services/abstract-das.service';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsDas extends AbstractDas<AppSettings> {
  constructor(injector: Injector) {
    super(injector, 'AppSettings');
  }
}
