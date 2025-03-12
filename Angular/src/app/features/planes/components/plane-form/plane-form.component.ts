import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Plane } from '../../model/plane';
import { BiaSharedModule } from '../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-plane-form',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
    ],
    imports: [BiaSharedModule]
})
export class PlaneFormComponent extends CrudItemFormComponent<Plane> {
  constructor(
    protected router: Router,
    protected activatedRoute: ActivatedRoute
  ) {
    super(router, activatedRoute);
  }
}
