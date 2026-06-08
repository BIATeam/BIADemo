import { Injectable } from '@angular/core';
import Dexie, { Table } from 'dexie';
import { HttpRequestItem } from 'packages/bia-ng/models/public-api';

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
