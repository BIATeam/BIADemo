import { Component, OnInit, Output, EventEmitter, OnDestroy, Injector } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute, Router } from '@angular/router';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemService } from '../../services/crud-item.service';
import { CrudConfig } from '../../model/crud-config';
import { PrimeTableColumn, PropType } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';

@Component({
  selector: 'bia-crud-item-edit',
  templateUrl: './crud-item-edit.component.html',
  styleUrls: ['./crud-item-edit.component.scss']
})
export class CrudItemEditComponent<CrudItem extends BaseDto> implements OnInit, OnDestroy {
  @Output() displayChange = new EventEmitter<boolean>();
  protected sub = new Subscription();
  public crudConfiguration : CrudConfig;
  public fields : PrimeTableColumn[];

  protected store: Store<AppState>;
  protected router: Router;
  protected activatedRoute: ActivatedRoute;
  protected biaTranslationService: BiaTranslationService;
  
  constructor(

    protected injector: Injector,
    public crudItemService: CrudItemService<CrudItem>,
  ) { 
    this.store = this.injector.get<Store<AppState>>(Store);
    this.router = this.injector.get<Router>(Router);
    this.activatedRoute = this.injector.get<ActivatedRoute>(ActivatedRoute);
    this.biaTranslationService = this.injector.get<BiaTranslationService>(BiaTranslationService);
  }

  ngOnInit() {
    this.initFieldsConfiguration();
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(event => {
          this.crudItemService.optionsService.loadAllOptions();
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

  protected initFieldsConfiguration() {
    this.sub.add(this.biaTranslationService.currentCultureDateFormat$.subscribe((dateFormat) => {
      this.fields = this.crudConfiguration.columns.map<PrimeTableColumn>(object => object.clone());
 
      this.fields.forEach(field => {
        switch (field.type)
        {
          case PropType.DateTime :
            field.formatDate = dateFormat.primeNgDateFormat;
            field.hourFormat = dateFormat.hourFormat;
            break;
          case PropType.Date :
            field.formatDate = dateFormat.primeNgDateFormat;
            break;
          case PropType.Time :
            field.formatDate = dateFormat.timeFormat;
            field.hourFormat = dateFormat.hourFormat;
            break;
          case PropType.TimeOnly :
            field.formatDate = dateFormat.timeFormat;
            field.hourFormat = dateFormat.hourFormat;
            break;
          case PropType.TimeSecOnly :
            field.formatDate = dateFormat.timeFormatSec;
            field.hourFormat = dateFormat.hourFormat;
            break;
        }
      });
    }));
  }
}
