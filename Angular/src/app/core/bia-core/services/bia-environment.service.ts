import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BiaEnvironmentService {
  protected static baseUrl: string | null = null;

  static getBaseUrl(): string {
    if (BiaEnvironmentService.baseUrl === null) {
      if (environment.apiUrlDynamic?.oldValue?.length > 0) {
        BiaEnvironmentService.baseUrl = window.location.origin.replace(
          environment.apiUrlDynamic?.oldValue,
          environment.apiUrlDynamic?.newValue
        );
      }

      BiaEnvironmentService.baseUrl = BiaEnvironmentService.baseUrl ?? '';
    }
    return BiaEnvironmentService.baseUrl as string;
  }

  static getApiUrl(): string {
    return BiaEnvironmentService.getBaseUrl() + environment.apiUrl;
  }

  static getHubUrl(): string {
    return BiaEnvironmentService.getBaseUrl() + environment.hubUrl;
  }

  static getServerLoggingUrl(): string {
    return (
      BiaEnvironmentService.getBaseUrl() +
      environment.logging.conf.serverLoggingUrl
    );
  }

  static getLoggingConf(): any {
    const conf: any = { ...environment.logging.conf };
    conf.serverLoggingUrl =
      BiaEnvironmentService.getBaseUrl() + conf.serverLoggingUrl;
    return conf;
  }
}
