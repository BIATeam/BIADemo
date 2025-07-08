import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { filter, first, map, Observable, skip, take } from 'rxjs';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { BaseDto } from 'src/app/shared/bia-shared/model/dto/base-dto';
import { clone } from 'src/app/shared/bia-shared/utils';
import { CrudItemFormComponent } from '../../components/crud-item-form/crud-item-form.component';
import { CrudItemSingleService } from '../../services/crud-item-single.service';
import { CrudItemComponent } from '../crud-item/crud-item.component';

@Component({
  selector: 'bia-crud-item-new',
  templateUrl: './crud-item-new.component.html',
  styleUrls: ['./crud-item-new.component.scss'],
  imports: [CrudItemFormComponent, AsyncPipe],
})
export class CrudItemNewComponent<CrudItem extends BaseDto>
  extends CrudItemComponent<CrudItem>
  implements OnInit, OnDestroy
{
  itemTemplate$?: Observable<CrudItem | undefined>;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    super(injector, crudItemService);

    const itemTemplateId: any | undefined =
      this.router.getCurrentNavigation()?.extras.state?.itemTemplateId;
    if (itemTemplateId) {
      this.itemTemplate$ = this.crudItemService.crudItem$.pipe(
        skip(1),
        take(1),
        map(crudItem => {
          const item = clone(crudItem);
          item.id = 0;
          return item;
        })
      );
      this.crudItemService.load(itemTemplateId);
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
