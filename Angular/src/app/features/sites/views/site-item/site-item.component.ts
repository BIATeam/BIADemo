import { AsyncPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {
  CrudItemItemComponent,
  CrudItemService,
  SpinnerComponent,
} from '@bia-team/bia-ng/shared';
import { Site } from '../../model/site';
import { SiteService } from '../../services/site.service';

@Component({
  selector: 'app-sites-item',
  templateUrl:
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../node_modules/@bia-team/bia-ng/templates/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: SiteService,
    },
  ],
})
export class SiteItemComponent
  extends CrudItemItemComponent<Site>
  implements OnInit {}
