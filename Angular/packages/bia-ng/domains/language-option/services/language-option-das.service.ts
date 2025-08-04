import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'bia-ng/core';
import { OptionDto } from 'bia-ng/models';

@Injectable({
  providedIn: 'root',
})
export class LanguageOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector) {
    super(injector, 'Languages');
  }
}
