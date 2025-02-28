import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudItemFormComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component';
import { Plane } from '../../model/plane';

@Component({
  selector: 'app-plane-form',
  templateUrl:
    '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.html',
  styleUrls: [
    '../../../../shared/bia-shared/feature-templates/crud-items/components/crud-item-form/crud-item-form.component.scss',
  ],
})
export class PlaneFormComponent extends CrudItemFormComponent<Plane> {
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    super();
  }

  onFormReadOnlyChanged(readOnly: boolean) {
    if (!readOnly) {
      this.router.navigate(['../edit'], { relativeTo: this.activatedRoute });
    }
  }
}
