import { Injectable } from '@angular/core';
import { AppSettings } from '../model/app-settings';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsService {
  appSettings: AppSettings;
}
