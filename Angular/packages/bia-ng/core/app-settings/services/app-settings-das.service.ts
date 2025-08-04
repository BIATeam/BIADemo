import { Injectable, Injector } from '@angular/core';
import { AppSettings } from 'bia-ng/models';
import { AbstractDas } from '../../services/abstract-das.service';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsDas extends AbstractDas<AppSettings> {
  constructor(injector: Injector) {
    super(injector, 'AppSettings');
  }
}
