import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { OptionDto } from 'biang/models';

@Injectable({
  providedIn: 'root',
})
export class SiteOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector) {
    super(injector, 'SiteOptions');
  }
}
