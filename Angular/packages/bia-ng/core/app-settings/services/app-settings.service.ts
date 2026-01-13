import { Injectable } from '@angular/core';
import { AppSettings } from '@bia-team/bia-ng/models';

@Injectable({
  providedIn: 'root',
})
export class AppSettingsService {
  public appSettings: AppSettings;
}
