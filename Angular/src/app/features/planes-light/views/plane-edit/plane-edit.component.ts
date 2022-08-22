import { Component } from '@angular/core';
import { Plane } from '../../model/plane';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AppState } from 'src/app/store/state';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { PlaneFacadeService } from '../../services/plane-facade.service';

@Component({
  selector: 'app-plane-edit',
  templateUrl: './plane-edit.component.html',
})
export class PlaneEditComponent extends CrudItemEditComponent<Plane> {
  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    protected biaTranslationService: BiaTranslationService,
    public facadeService: PlaneFacadeService, 
  ) {
    super(store,router,activatedRoute,biaTranslationService,facadeService);
  }
}
