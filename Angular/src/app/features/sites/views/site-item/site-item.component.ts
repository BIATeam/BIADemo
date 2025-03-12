import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';
import { RouterOutlet } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { SpinnerComponent } from '../../../../shared/bia-shared/components/spinner/spinner.component';

@Component({
    selector: 'app-sites-item',
    templateUrl: '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
    styleUrls: [
        '../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
    ],
    imports: [RouterOutlet, NgIf, SpinnerComponent, AsyncPipe]
})
export class SiteItemComponent
  extends CrudItemItemComponent<Site>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public siteService: SiteService
  ) {
    super(injector, siteService);
  }
}
