import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'biang/core';
import { Engine } from '../model/engine';

@Injectable({
  providedIn: 'root',
})
export class EngineDas extends AbstractDas<Engine> {
  constructor(injector: Injector) {
    super(injector, 'Engines');
  }
}
