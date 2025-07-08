import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { SiteFormComponent } from '../../components/site-form/site-form.component';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';
import { siteCRUDConfiguration } from '../../site.constants';

@Component({
  selector: 'app-site-edit',
  templateUrl: './site-edit.component.html',
  imports: [NgIf, SiteFormComponent, AsyncPipe, SpinnerComponent],
})
export class SiteEditComponent extends CrudItemEditComponent<Site> {
  constructor(
    protected injector: Injector,
    public siteService: SiteService
  ) {
    super(injector, siteService);
    this.crudConfiguration = siteCRUDConfiguration;
  }
}
