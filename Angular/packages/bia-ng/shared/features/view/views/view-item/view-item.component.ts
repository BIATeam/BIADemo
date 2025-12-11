import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemService } from '../../../../feature-templates/crud-items/services/crud-item.service';
import { CrudItemItemComponent } from '../../../../feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { View } from '../../model/view';
import { ViewService } from '../../services/view.service';

@Component({
  selector: 'bia-views-item',
  templateUrl:
    '../../../../feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, AsyncPipe, SpinnerComponent],
  providers: [
    {
      provide: CrudItemService,
      useExisting: ViewService,
    },
  ],
})
export class ViewItemComponent extends CrudItemItemComponent<View> {}
