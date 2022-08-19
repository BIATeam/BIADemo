import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { BaseDto } from '../../../model/base-dto';

@Injectable({
  providedIn: 'root'
})
export class CrudItemDas<CrudItem extends BaseDto> extends AbstractDas<CrudItem> {
  constructor(injector: Injector) {
    super(injector, 'CrudItems');
  }
}
