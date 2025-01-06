import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { Engine } from '../model/engine';

@Injectable({
  providedIn: 'root',
})
export class EngineDas extends AbstractDas<Engine> {
  constructor(injector: Injector) {
    super(injector, 'Engines');
  }
}
