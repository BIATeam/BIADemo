import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { filter, first } from 'rxjs';
import { biaSuccessWaitRefreshSignalR } from 'src/app/core/bia-core/shared/bia-action';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
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
  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    super(injector, crudItemService);
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
