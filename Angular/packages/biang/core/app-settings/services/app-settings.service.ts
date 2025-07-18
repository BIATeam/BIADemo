import { Injectable } from '@angular/core';
import { AppSettings } from 'biang/models';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsService {
  public appSettings: AppSettings;
}
