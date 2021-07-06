import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

@Injectable({
  providedIn: 'root'
})
export class PlaneTypeOptionDas extends AbstractDas<OptionDto> {
  constructor(injector: Injector) {
    super(injector, 'PlanesTypes');
  }
}


















