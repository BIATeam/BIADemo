import { BiaNavigation } from 'biang/models';

export class BiaAppConstantsService {
  public static navigation: BiaNavigation[] = [];
  public static supportedTranslations: string[] = [];
  public static defaultPageSize: number = 10;
  public static teamTypeRightPrefix: { key: number; value: string }[] = [];
  public static defaultTranslations: number[] = [];

  static init(
    navigation: BiaNavigation[],
    supportedTranslations: string[],
    defaultPageSize: number,
    teamTypeRightPrefix: { key: number; value: string }[],
    defaultTranslations: number[]
  ) {
    BiaAppConstantsService.navigation = navigation;
    BiaAppConstantsService.supportedTranslations = supportedTranslations;
    BiaAppConstantsService.defaultPageSize = defaultPageSize;
    BiaAppConstantsService.teamTypeRightPrefix = teamTypeRightPrefix;
    BiaAppConstantsService.defaultTranslations = defaultTranslations;
  }
}
