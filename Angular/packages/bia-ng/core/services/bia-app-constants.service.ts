import {
  AllEnvironments,
  AppEnvironment,
  BiaNavigation,
} from 'packages/bia-ng/models/public-api';

export class BiaAppConstantsService {
  public static allEnvironments: AllEnvironments;
  public static environment: AppEnvironment;
  public static navigation: BiaNavigation[] = [];
  public static supportedTranslations: string[] = [];
  public static defaultPageSize: number = 10;
  public static teamTypeRightPrefix: { key: number; value: string }[] = [];
  public static defaultTranslations: number[] = [];
  public static defaultPopupMinWidth: string = '60vw';
  public static showFps: boolean = false;

  public static storageAppSettingsKey = () => {
    return `${BiaAppConstantsService.allEnvironments.companyName}.${BiaAppConstantsService.allEnvironments.appTitle}.AppSettings`;
  };
  public static storageBiaUserConfigKey = () =>
    `${BiaAppConstantsService.allEnvironments.companyName}.${BiaAppConstantsService.allEnvironments.appTitle}.bia-user-config`;

  static init(
    allEnvironments: AllEnvironments,
    environment: AppEnvironment,
    navigation: BiaNavigation[],
    supportedTranslations: string[],
    defaultPageSize: number,
    teamTypeRightPrefix: { key: number; value: string }[],
    defaultTranslations: number[],
    defaultPopupMinWidth: string,
    showFps: boolean
  ) {
    BiaAppConstantsService.allEnvironments = allEnvironments;
    BiaAppConstantsService.environment = environment;
    BiaAppConstantsService.navigation = navigation;
    BiaAppConstantsService.supportedTranslations = supportedTranslations;
    BiaAppConstantsService.defaultPageSize = defaultPageSize;
    BiaAppConstantsService.teamTypeRightPrefix = teamTypeRightPrefix;
    BiaAppConstantsService.defaultTranslations = defaultTranslations;
    BiaAppConstantsService.defaultPopupMinWidth = defaultPopupMinWidth;
    BiaAppConstantsService.showFps = showFps;
  }
}
