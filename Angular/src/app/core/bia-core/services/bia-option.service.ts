import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';

export class BiaOptionService {

  public static Clone<T>(value: any) {
    if (!value) {
      return null;
    }
    return <T>{ ...value };
  }

  public static Differential<T extends BaseDto>(newList: T[], oldList: T[]) {
    let differential: T[] = [];
    if (oldList && Array.isArray(oldList)) {
      // Delete items
      const toDeletedConnectingAirports = oldList
        .filter((s) => !newList || !newList.map(x => x.id).includes(s.id))
        .map((s) => <T>{ ...s, dtoState: DtoState.Deleted });

      if (toDeletedConnectingAirports) {
        differential = differential.concat(toDeletedConnectingAirports);
      }
    }

    if (newList) {
      // Add items
      const toAddedConnectingAirports = newList
        ?.filter((s) => !oldList || !oldList.map(x => x.id).includes(s.id))
        .map((s) => <T>{ ...s, dtoState: DtoState.Added });

      if (toAddedConnectingAirports) {
        differential = differential.concat(toAddedConnectingAirports);
      }
    }
    return differential;
  }
}
