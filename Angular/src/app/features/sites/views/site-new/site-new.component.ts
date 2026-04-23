import { AsyncPipe } from '@angular/common';
import { Component, Injector } from '@angular/core';
import { AuthService } from 'packages/bia-ng/core/public-api';
import { CrudItemNewComponent } from 'packages/bia-ng/shared/public-api';
import { SiteFormComponent } from '../../components/site-form/site-form.component';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';
import { siteCRUDConfiguration } from '../../site.constants';

@Component({
  selector: 'app-site-new',
  templateUrl: './site-new.component.html',
  imports: [SiteFormComponent, AsyncPipe],
})
export class SiteNewComponent extends CrudItemNewComponent<Site> {
  constructor(
    protected injector: Injector,
    public siteService: SiteService,
    protected authService: AuthService
  ) {
    super(injector, siteService);
    this.crudConfiguration = siteCRUDConfiguration;
  }

  onSubmitted(crudItemToCreate: Site): void {
    super.onSubmitted(crudItemToCreate);
    this.authService.reLogin();
  }
}
