import { DtoState, PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BiaAdvancedFilterConfig,
  BiaAdvancedFilterFieldConfig,
  OptionDto,
} from 'packages/bia-ng/models/public-api';

export class PlaneAdvancedFilterDto {
  enginesNumberRange: number | null;

  static hasFilter(filter: PlaneAdvancedFilterDto): boolean {
    return (
      !!filter &&
      filter.enginesNumberRange !== null &&
      filter.enginesNumberRange !== undefined
    );
  }
}

export const planeAdvancedFilterConfig: BiaAdvancedFilterConfig<PlaneAdvancedFilterDto> =
  {
    fields: [
      Object.assign(
        new BiaAdvancedFilterFieldConfig<PlaneAdvancedFilterDto>(
          'enginesNumberRange',
          'plane.enginesNumberRange.title',
          PropType.OneToMany
        ),
        {
          options: [
            new OptionDto(
              0,
              'plane.enginesNumberRange.zero',
              DtoState.Unchanged
            ),
            new OptionDto(
              1,
              'plane.enginesNumberRange.oneOrTwo',
              DtoState.Unchanged
            ),
            new OptionDto(
              2,
              'plane.enginesNumberRange.threeToFive',
              DtoState.Unchanged
            ),
            new OptionDto(
              3,
              'plane.enginesNumberRange.sixOrMore',
              DtoState.Unchanged
            ),
          ],
          allowSelectFilter: false,
        } as Partial<BiaAdvancedFilterFieldConfig<PlaneAdvancedFilterDto>>
      ),
    ],
  };
