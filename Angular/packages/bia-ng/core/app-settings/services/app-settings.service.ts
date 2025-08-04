import { Injectable } from '@angular/core';
import { AppSettings } from 'packages/bia-ng/models/public-api';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsService {
  public appSettings: AppSettings;
}
