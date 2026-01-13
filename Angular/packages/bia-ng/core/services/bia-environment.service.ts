import { Injectable } from '@angular/core';
import { BiaAppConstantsService } from './bia-app-constants.service';

@Injectable({
  providedIn: 'root',
})
export class BiaEnvironmentService {
  protected static baseUrl: string | null = null;

  public static getBaseUrl(): string {
    if (BiaEnvironmentService.baseUrl === null) {
      if (
        (BiaAppConstantsService.environment.apiUrlDynamic?.oldValue?.length ??
          0) > 0
      ) {
        BiaEnvironmentService.baseUrl = window.location.origin.replace(
          BiaAppConstantsService.environment.apiUrlDynamic?.oldValue ?? '',
          BiaAppConstantsService.environment.apiUrlDynamic?.newValue ?? ''
        );
      }

      BiaEnvironmentService.baseUrl = BiaEnvironmentService.baseUrl ?? '';
    }
    return BiaEnvironmentService.baseUrl as string;
  }

  public static getApiUrl(): string {
    return (
      BiaEnvironmentService.getBaseUrl() +
      BiaAppConstantsService.environment.apiUrl
    );
  }

  public static getHubUrl(): string {
    return (
      BiaEnvironmentService.getBaseUrl() +
      BiaAppConstantsService.environment.hubUrl
    );
  }

  public static getServerLoggingUrl(): string {
    return (
      BiaEnvironmentService.getBaseUrl() +
      BiaAppConstantsService.environment.logging.conf.serverLoggingUrl
    );
  }

  public static getLoggingConf(): any {
    const conf: any = { ...BiaAppConstantsService.environment.logging.conf };
    conf.serverLoggingUrl =
      BiaEnvironmentService.getBaseUrl() + conf.serverLoggingUrl;
    return conf;
  }
}
