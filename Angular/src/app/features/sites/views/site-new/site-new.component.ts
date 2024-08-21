import { Component, Injector } from '@angular/core';
import { Site } from '../../model/site';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { SiteService } from '../../services/site.service';
import { siteCRUDConfiguration } from '../../site.constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';

@Component({
  selector: 'app-site-new',
  templateUrl: './site-new.component.html',
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
