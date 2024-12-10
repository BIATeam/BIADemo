import { Injectable } from '@angular/core';
import { Table } from 'dexie';
import { BiaDatabase } from '../../shared/bia-shared/data/bia.database';
import { HttpRequestItem } from './models/http-request-item';

export interface DataItem {
  url: string;
  data: any;
}

@Injectable({
  providedIn: 'root',
})
export class BiaOfflineDatabase extends BiaDatabase {
  public httpRequests!: Table<HttpRequestItem, number>;
  public datas!: Table<DataItem, string>;

  constructor() {
    super('biaOfflineDB');
    this.init();
  }

  protected defineSchemas(): void {
    this.defineSchemaVersion(3, {
      httpRequests: '++id',
      datas: 'url',
    });
  }
}
