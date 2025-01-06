import { Component, Injector } from '@angular/core';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';
import { siteCRUDConfiguration } from '../../site.constants';

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
