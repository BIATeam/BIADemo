import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { OptionDto } from '@bia-team/bia-ng/models';

@Injectable({
  providedIn: 'root',
})
export class AnnouncementTypeOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector) {
    super(injector, 'AnnouncementTypeOptions');
  }
}
