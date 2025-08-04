import { Injectable } from '@angular/core';
import { AppSettings } from 'bia-ng/models';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsService {
  public appSettings: AppSettings;
}
