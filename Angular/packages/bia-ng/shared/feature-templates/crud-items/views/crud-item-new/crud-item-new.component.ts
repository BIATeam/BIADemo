import { AsyncPipe } from '@angular/common';
import {
  Component,
  computed,
  Injector,
  OnDestroy,
  OnInit,
} from '@angular/core';
import {
  biaSuccessWaitRefreshSignalR,
  clone,
} from 'packages/bia-ng/core/public-api';
import { BaseDto } from 'packages/bia-ng/models/public-api';
import { filter, first, map, Observable, skip, take } from 'rxjs';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { CrudItemSingleService } from '../../services/crud-item-single.service';
import { CrudItemComponent } from '../crud-item/crud-item.component';

@Component({
  selector: 'bia-crud-item-new',
  templateUrl: './crud-item-new.component.html',
  styleUrls: ['./crud-item-new.component.scss'],
  imports: [CrudItemFormComponent, AsyncPipe],
})
export class CrudItemNewComponent<CrudItem extends BaseDto<string | number>>
  extends CrudItemComponent<CrudItem>
  implements OnInit, OnDestroy
{
  itemTemplate$?: Observable<CrudItem | undefined>;
  readonly itemTemplateId = computed(() => {
    const nav = this.router.currentNavigation();
    return nav?.extras.state?.itemTemplateId as string | undefined;
  });

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    super(injector, crudItemService);

    if (this.itemTemplateId()) {
      this.itemTemplate$ = this.crudItemService.crudItem$.pipe(
        skip(1),
        take(1),
        map(crudItem => {
          const item = clone(crudItem);
          if (typeof item.id === 'number') {
            item.id = 0;
          } else if (typeof item.id === 'string') {
            item.id = '';
          }
          return item;
        })
      );
      this.crudItemService.load(this.itemTemplateId());
    }
  }

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.crudItemService.optionsService.loadAllOptions(
          this.crudConfiguration.optionFilter
        );
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(crudItemToCreate: CrudItem) {
    const successActionType = this.crudItemService.createSuccessActionType;

    if (successActionType) {
      this.actions
        .pipe(
          filter(
            (action: any) =>
              action.type === successActionType ||
              action.type === biaSuccessWaitRefreshSignalR.type
          ),
          first()
        )
        .subscribe(() => {
          this.navigateBack();
        });
    }

    this.crudItemService.create(crudItemToCreate);

    if (!successActionType) {
      this.navigateBack();
    }
  }

  protected navigateBack() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
