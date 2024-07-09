import { Component, Injector } from '@angular/core';
import { Site } from '../../model/site';
import { siteCRUDConfiguration } from '../../site.constants';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { SiteService } from '../../services/site.service';

@Component({
  selector: 'app-site-edit',
  templateUrl: './site-edit.component.html',
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
