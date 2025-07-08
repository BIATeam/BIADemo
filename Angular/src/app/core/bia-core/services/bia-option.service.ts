import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';

export class BiaOptionService {
  public static clone<T>(value: any) {
    if (!value) {
      return null as any;
    }
    return <T>{ ...value };
  }

  public static differential<T extends BaseDto>(newList: T[], oldList: T[]) {
    let differential: T[] = [];
    if (oldList && Array.isArray(oldList)) {
      // Delete items
      const toDeleteds = oldList
        .filter(s => !newList || !newList.map(x => x.id).includes(s.id))
        .map(s => <T>{ ...s, dtoState: DtoState.Deleted });

      if (toDeleteds) {
        differential = differential.concat(toDeleteds);
      }
    }

    if (newList) {
      // Add items
      const toAddeds = newList
        .filter(s => !oldList || !oldList.map(x => x.id).includes(s.id))
        .map(s => <T>{ ...s, dtoState: DtoState.Added });

      if (toAddeds) {
        differential = differential.concat(toAddeds);
      }
    }

    if (oldList && newList) {
      // Other Items
      const unchangeds = oldList.filter(n =>
        newList.map(o => o.id).includes(n.id)
      );

      if (unchangeds) {
        differential = differential.concat(unchangeds);
      }
    }

    return differential;
  }
}
