import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from '@bia-team/bia-ng/core';
import { Engine, engineFieldsConfiguration } from '../model/engine';

@Injectable({
  providedIn: 'root',
})
export class EngineDas extends AbstractDas<Engine> {
  constructor(injector: Injector) {
    super(injector, 'Engines', engineFieldsConfiguration);
  }
}
