import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { OptionDto } from 'biang/models';

@Injectable({
  providedIn: 'root',
})
export class PartOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector) {
    super(injector, 'Parts');
  }
}
