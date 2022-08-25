import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Plane, PlaneCRUDConfiguration } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { ActivatedRoute, Router } from '@angular/router';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-new',
  templateUrl: './plane-new.component.html',
})
export class PlaneNewComponent extends CrudItemNewComponent<Plane>  {
   constructor(
    protected store: Store<AppState>,
    protected router: Router,
    protected activatedRoute: ActivatedRoute,
    protected biaTranslationService: BiaTranslationService,
    public planeService: PlaneService, 
  ) {
     super(store,router,activatedRoute,biaTranslationService, planeService);
     this.crudConfiguration = PlaneCRUDConfiguration;
   }
}