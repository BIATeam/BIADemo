import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import { Engine, engineFieldsConfiguration } from '../model/engine';

@Injectable({
  providedIn: 'root',
})
export class EngineDas extends AbstractDas<Engine> {
  constructor(injector: Injector) {
    super(injector, 'Engines', engineFieldsConfiguration);
  }
}
