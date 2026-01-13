import { Injectable } from '@angular/core';
import { HttpRequestItem } from '@bia-team/bia-ng/models';
import Dexie, { Table } from 'dexie';

export interface DataItem {
  url: string;
  data: any;
}

@Injectable({
  providedIn: 'root',
})
export class AppDB extends Dexie {
  public httpRequests!: Table<HttpRequestItem, number>;
  public datas!: Table<DataItem, string>;

  constructor() {
    super('BIADemoDB');
    this.version(3).stores({
      httpRequests: '++id',
      datas: 'url',
    });
  }
}
