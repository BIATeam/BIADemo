import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BiaFormComponent } from 'src/app/shared/bia-shared/components/form/bia-form/bia-form.component';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-form',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
  imports: [BiaFormComponent],
})
export class PlaneFormComponent extends CrudItemFormComponent<Plane> {
  constructor(
    protected router: Router,
    protected activatedRoute: ActivatedRoute
  ) {
    super(router, activatedRoute);
  }
}
