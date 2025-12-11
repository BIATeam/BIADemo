import { DtoState } from '../model/dto-state.enum';
import { BaseDto } from '../model/dto/base-dto';

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

  public static scrollHorizontalToElementInTable(
    element: HTMLElement | undefined
  ) {
    if (!element) return;

    const container = element.closest(
      '.p-datatable-table-container'
    ) as HTMLElement | null;
    if (!container) return;

    const datatable = element.closest('.p-datatable') as HTMLElement | null;
    if (!datatable) return;

    const thead = datatable.querySelector(
      '.p-datatable-thead'
    ) as HTMLElement | null;
    if (!thead) return;

    const frozenLeftCols = Array.from(
      thead.querySelectorAll(
        'th.p-datatable-frozen-column.p-datatable-frozen-column-left'
      )
    ) as HTMLElement[];

    const frozenLeftWidth = frozenLeftCols.reduce(
      (sum, col) => sum + col.getBoundingClientRect().width,
      0
    );

    const frozenRightCols = Array.from(
      thead.querySelectorAll(
        'th.p-datatable-frozen-column.p-datatable-frozen-column-right'
      )
    ) as HTMLElement[];

    const frozenRightWidth = frozenRightCols.reduce(
      (sum, col) => sum + col.getBoundingClientRect().width,
      0
    );

    const containerRect = container.getBoundingClientRect();
    const visibleLeft = containerRect.left + frozenLeftWidth;
    const visibleRight = containerRect.right - frozenRightWidth;

    const elRect = element.getBoundingClientRect();
    const isLeftHidden = elRect.left < visibleLeft;
    const isRightHidden = elRect.right > visibleRight;

    if (!isLeftHidden && !isRightHidden) return;

    let newScrollLeft = container.scrollLeft;

    if (isLeftHidden) {
      newScrollLeft -= visibleLeft - elRect.left;
    } else if (isRightHidden) {
      newScrollLeft += elRect.right - visibleRight;
    }

    const maxScrollLeft = Math.max(
      0,
      container.scrollWidth - container.clientWidth
    );

    newScrollLeft = Math.max(0, Math.min(newScrollLeft, maxScrollLeft));

    container.scrollTo({
      left: newScrollLeft,
      behavior: 'auto',
    });
  }
}
