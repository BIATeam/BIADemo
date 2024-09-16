import { Injectable } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { BaseDto } from '../../../model/base-dto';
import { CrudItemOptionsService } from './crud-item-options.service';
import { CrudItemSignalRService } from './crud-item-signalr.service';
import { CrudListAndItemService } from './crud-list-and-item.service';

@Injectable({
  providedIn: 'root',
})
export abstract class CrudItemService<
  CrudItem extends BaseDto,
> extends CrudListAndItemService<CrudItem, CrudItem> {
  constructor(
    public dasService: AbstractDas<CrudItem>,
    public signalRService: CrudItemSignalRService<CrudItem>,
    public optionsService: CrudItemOptionsService
  ) {
    super(dasService, signalRService, optionsService);
  }
}
