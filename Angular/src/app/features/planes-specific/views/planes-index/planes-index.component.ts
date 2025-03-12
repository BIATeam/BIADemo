import { Component, Injector, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { Permission } from 'src/app/shared/permission';
import { PlaneTableComponent } from '../../components/plane-table/plane-table.component';
import { Plane } from '../../model/plane';
import { PlaneSpecific } from '../../model/plane-specific';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneService } from '../../services/plane.service';
import { NgClass, NgSwitch, NgSwitchCase, NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';
import { PrimeTemplate } from 'primeng/api';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-planes-specific-index',
    templateUrl: './planes-index.component.html',
    styleUrls: ['./planes-index.component.scss'],
    imports: [NgClass, BiaSharedModule, PrimeTemplate, NgSwitch, NgSwitchCase, NgIf, AsyncPipe, TranslateModule]
})
export class PlanesIndexComponent extends CrudItemsIndexComponent<
  Plane,
  PlaneSpecific
> {
  @ViewChild(PlaneTableComponent, { static: false })
  crudItemTableComponent: PlaneTableComponent;

  constructor(
    protected injector: Injector,
    public planeService: PlaneService,
    protected authService: AuthService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeCRUDConfiguration;
  }

  protected setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Plane_Update);
    this.canDelete = this.authService.hasPermission(Permission.Plane_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Plane_Create);
  }
}
