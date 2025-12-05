import { BiaOptionService } from 'packages/bia-ng/core/public-api';
import { DtoState, PropType } from 'packages/bia-ng/models/enum/public-api';
import {
  BaseDto,
  BiaFieldConfig,
  OptionDto,
} from 'packages/bia-ng/models/public-api';

export class CrudHelperService {
  public static readonly newIdStartingValue: number = -1;
  public static readonly uniqueStringIdentifier = 'UNIQUE_STRING_IDENTIFIER_';

  public static onEmbeddedItemSave<T extends BaseDto<string | number>>(
    embeddedItem: T,
    embeddedItemArray: T[],
    newId: number
  ): number {
    if (embeddedItem.id !== 0 && embeddedItem.id !== '') {
      embeddedItem.dtoState =
        embeddedItem.dtoState === DtoState.Unchanged
          ? DtoState.Modified
          : embeddedItem.dtoState;
      embeddedItemArray[
        embeddedItemArray.findIndex(
          oldEmbeddedItem => embeddedItem.id === oldEmbeddedItem.id
        )
      ] = embeddedItem;
    } else {
      if (CrudHelperService.typeofId(embeddedItem) === 'number') {
        embeddedItem.id = newId;
      } else {
        embeddedItem.id =
          CrudHelperService.uniqueStringIdentifier + newId.toString();
      }
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
      element.id =
        element.id && (element.id as string) !== '' ? element.id : '';
    }
  }

  public static ApplyDiff<TDto extends { id: number | string }>(
    oldElement: TDto | undefined,
    newElement: TDto,
    fields: BiaFieldConfig<TDto>[]
  ) {
    for (const field of fields) {
      switch (field.type) {
        case PropType.Boolean:
          Reflect.set(
            newElement,
            field.field,
            newElement[field.field] ? newElement[field.field] : false
          );
          break;
        case PropType.ManyToMany:
          Reflect.set(
            newElement,
            field.field,
            BiaOptionService.differential(
              Reflect.get(newElement, field.field) as BaseDto[],
              (oldElement && oldElement.id
                ? (Reflect.get(oldElement, field.field) ?? [])
                : []) as BaseDto[]
            )
          );
          break;
        case PropType.OneToMany:
          if (
            field.isEditableChoice &&
            typeof newElement[field.field as keyof TDto] === 'string'
          ) {
            Reflect.set(
              newElement,
              field.field,
              new OptionDto(
                0,
                newElement[field.field as keyof TDto] as string,
                DtoState.AddedNewChoice
              )
            );
          } else {
            Reflect.set(
              newElement,
              field.field,
              BiaOptionService.clone(newElement[field.field as keyof TDto])
            );
          }
          break;
      }
    }
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
