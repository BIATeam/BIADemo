import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { AppState } from 'src/app/store/state';
import { CrudConfig } from '../../model/crud-config';
import { CrudItemSingleService } from '../../services/crud-item-single.service';

@Component({
  selector: 'bia-crud-item-edit',
  templateUrl: './crud-item-edit.component.html',
  styleUrls: ['./crud-item-edit.component.scss'],
})
export class CrudItemEditComponent<CrudItem extends BaseDto>
  implements OnInit, OnDestroy
{
  @Output() displayChange = new EventEmitter<boolean>();
  protected sub = new Subscription();
  public crudConfiguration: CrudConfig;

  protected store: Store<AppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected biaTranslationService: BiaTranslationService;

  constructor(
    protected injector: Injector,
    public crudItemService: CrudItemSingleService<CrudItem>
  ) {
    this.store = this.injector.get<Store<AppState>>(Store);
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.biaTranslationService = this.injector.get<BiaTranslationService>(
      BiaTranslationService
    );
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

  onSubmitted(crudItemToUpdate: CrudItem) {
    this.crudItemService.update(crudItemToUpdate);
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../../'], { relativeTo: this.activatedRoute });
  }
}
