import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Plane, PlaneCRUDConfiguration } from '../../model/plane';
import { AppState } from 'src/app/store/state';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { PlaneFacadeService } from '../../services/plane-facade.service';

@Component({
  selector: 'app-planes-index',
  templateUrl: './planes-index.component.html',
  styleUrls: ['./planes-index.component.scss']
})

export class PlanesIndexComponent extends CrudItemsIndexComponent<Plane> {
  constructor(
    protected store: Store<AppState>,
    protected router: Router,
    public activatedRoute: ActivatedRoute,
    protected authService: AuthService,
    protected translateService: TranslateService,
    protected biaTranslationService: BiaTranslationService,
    protected facadeService: PlaneFacadeService, 
  ) {
    super(store,router,activatedRoute,translateService,
      biaTranslationService,facadeService);
    this.crudConfiguration = PlaneCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }
}
