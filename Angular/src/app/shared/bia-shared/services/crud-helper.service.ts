import { BaseDto } from '../model/base-dto';
import { DtoState } from '../model/dto-state.enum';

export class CrudHelperService {
  public static readonly newIdStartingValue: number = -1;

  public static onEmbeddedItemSave<T extends BaseDto>(
    embeddedItem: T,
    embeddedItemArray: T[],
    newId: number
  ): number {
    if (embeddedItem.id !== 0) {
      embeddedItem.dtoState = DtoState.Modified;
      embeddedItemArray[
        embeddedItemArray.findIndex(
          oldEmbeddedItem => embeddedItem.id == oldEmbeddedItem.id
        )
      ] = embeddedItem;
    } else {
      embeddedItem.id = newId;
      newId--;
      embeddedItemArray.push(embeddedItem);
    }
    return newId;
  }
}
