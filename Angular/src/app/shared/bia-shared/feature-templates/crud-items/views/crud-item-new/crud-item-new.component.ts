import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute, Router } from '@angular/router';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Subscription } from 'rxjs';
import { BaseDto } from 'src/app/shared/bia-shared/model/base-dto';
import { CrudItemFacadeService } from '../../services/crud-item-facade.service';
import { CrudConfig } from '../../model/crud-config';

@Component({
  selector: 'app-crud-item-new',
  templateUrl: './crud-item-new.component.html',
  styleUrls: ['./crud-item-new.component.scss']
})
export class CrudItemNewComponent<CrudItem extends BaseDto> implements OnInit, OnDestroy  {
  protected sub = new Subscription();
  public crudConfiguration : CrudConfig;

  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    protected biaTranslationService: BiaTranslationService,
    public facadeService: CrudItemFacadeService<CrudItem>,
  ) {}

  ngOnInit() {
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(event => {
          this.facadeService.optionsService.loadAllOptions();
      })
    );
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  onSubmitted(crudItemToCreate: CrudItem) {
    this.facadeService.create(crudItemToCreate);
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }

  onCancelled() {
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
