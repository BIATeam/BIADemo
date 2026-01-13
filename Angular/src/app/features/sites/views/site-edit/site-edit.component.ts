import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import {
  CrudItemEditComponent,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { SiteFormComponent } from '../../components/site-form/site-form.component';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';
import { siteCRUDConfiguration } from '../../site.constants';

@Component({
  selector: 'app-site-edit',
  templateUrl: './site-edit.component.html',
  imports: [SiteFormComponent, AsyncPipe, SpinnerComponent],
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
