import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { AppSettings } from '../model/app-settings';

@Injectable({
  providedIn: 'root'
})

export class AppSettingsDas extends AbstractDas<AppSettings> {
  constructor(injector: Injector) {
    super(injector, 'AppSettings');
  }
}












