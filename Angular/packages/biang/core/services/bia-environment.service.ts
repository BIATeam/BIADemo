import { Inject, Injectable } from '@angular/core';
import { AllEnvironments, AppEnvironment } from 'biang/models';

@Injectable({
  providedIn: 'root',
})
export class BiaEnvironmentService {
  protected static baseUrl: string | null = null;
  public static environment: AppEnvironment;
  public static allEnvironments: AllEnvironments;

  constructor(
    @Inject('environment')
    environment: AppEnvironment,
    @Inject('allEnvironments')
    allEnvironments: AllEnvironments
  ) {
    BiaEnvironmentService.environment = environment;
    BiaEnvironmentService.allEnvironments = allEnvironments;
  }

  public static getBaseUrl(): string {
    if (BiaEnvironmentService.baseUrl === null) {
      if (
        (BiaEnvironmentService.environment.apiUrlDynamic?.oldValue?.length ??
          0) > 0
      ) {
        BiaEnvironmentService.baseUrl = window.location.origin.replace(
          BiaEnvironmentService.environment.apiUrlDynamic?.oldValue ?? '',
          BiaEnvironmentService.environment.apiUrlDynamic?.newValue ?? ''
        );
      }

      BiaEnvironmentService.baseUrl = BiaEnvironmentService.baseUrl ?? '';
    }
    return BiaEnvironmentService.baseUrl as string;
  }

  public static getApiUrl(): string {
    return (
      BiaEnvironmentService.getBaseUrl() +
      BiaEnvironmentService.environment.apiUrl
    );
  }

  public static getHubUrl(): string {
    return (
      BiaEnvironmentService.getBaseUrl() +
      BiaEnvironmentService.environment.hubUrl
    );
  }

  public static getServerLoggingUrl(): string {
    return (
      BiaEnvironmentService.getBaseUrl() +
      BiaEnvironmentService.environment.logging.conf.serverLoggingUrl
    );
  }

  public static getLoggingConf(): any {
    const conf: any = { ...BiaEnvironmentService.environment.logging.conf };
    conf.serverLoggingUrl =
      BiaEnvironmentService.getBaseUrl() + conf.serverLoggingUrl;
    return conf;
  }
}
