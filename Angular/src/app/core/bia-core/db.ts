import { Injectable } from '@angular/core';
import Dexie, { Table } from 'dexie';
import { HttpRequestItem } from './services/online-offline.service';

export interface DataItem {
  url: string;
  data: any;
}

@Injectable({
  providedIn: 'root'
})
export class AppDB extends Dexie {
  public httpRequests!: Table<HttpRequestItem, number>;
  public datas!: Table<DataItem, number>;

  constructor() {
    super('biaDemoDB');
    this.version(3).stores({
      httpRequests: '++id',
      datas: '++'
    });
  }
}
