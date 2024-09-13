import { Injector } from '@angular/core';
import { AbstractDasWithListAndItem } from './abstract-das-with-list-and-item.service';

export abstract class AbstractDas<
  TOut,
  TIn = Pick<TOut, Exclude<keyof TOut, 'id'>>,
> extends AbstractDasWithListAndItem<TOut, TOut, TIn> {
  constructor(injector: Injector, endpoint: string) {
    super(injector, endpoint);
  }
}
