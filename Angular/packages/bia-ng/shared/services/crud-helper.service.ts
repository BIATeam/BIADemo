import { DtoState } from 'packages/bia-ng/models/enum/public-api';
import { BaseDto } from 'packages/bia-ng/models/public-api';

export class CrudHelperService {
  public static readonly newIdStartingValue: number = -1;

  public static onEmbeddedItemSave<T extends BaseDto<string | number>>(
    embeddedItem: T,
    embeddedItemArray: T[],
    newId: number
  ): number {
    if (embeddedItem.id !== 0) {
      embeddedItem.dtoState = DtoState.Modified;
      embeddedItemArray[
        embeddedItemArray.findIndex(
          oldEmbeddedItem => embeddedItem.id === oldEmbeddedItem.id
        )
      ] = embeddedItem;
    } else {
      embeddedItem.id = newId;
      newId--;
      embeddedItemArray.push(embeddedItem);
    }
    return newId;
  }

  public static typeofId<TDto extends { id: number | string }>(
    element: TDto
  ): string {
    if (element.id !== null && element.id !== undefined) {
      if (typeof element.id === 'number') {
        return 'number';
      } else if (typeof element.id === 'string') {
        return 'string';
      }
    }
    return 'number';
  }

  public static initId<TDto extends { id: number | string }>(element: TDto) {
    if (CrudHelperService.typeofId(element) === 'number') {
      element.id = element.id && (element.id as number) > 0 ? element.id : 0;
    } else if (CrudHelperService.typeofId(element) === 'string') {
      element.id = element.id > '' ? element.id : '';
    }
  }
}
