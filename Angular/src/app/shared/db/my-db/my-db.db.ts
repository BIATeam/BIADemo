import { Injectable } from '@angular/core';
import { Table } from 'dexie';
import { BiaDatabase } from 'src/app/core/bia-core/bia.database';
import { MyDocument } from './entities/document.entity';
import { User } from './entities/user.entity';
import { Warehouse } from './entities/warehouse.entity';

@Injectable()
export class MyDb extends BiaDatabase {
  users!: Table<User, number>;
  warehouses!: Table<Warehouse, number>;
  documents!: Table<MyDocument, string>;

  constructor() {
    super('MyDb');
  }

  protected defineSchemas(): void {
    // Initial schema
    this.defineSchemaVersion(1, {
      users: '++id, name, email',
      warehouses: '++id, name',
    });

    // Delete table
    this.defineSchemaVersion(2, {
      warehouses: null,
    });

    // Add table property
    this.defineSchemaVersion(
      3,
      {
        users: '++id, name, email, newProperty',
      },
      async trans => {
        await trans
          .table('users')
          .toCollection()
          .modify(user => {
            user.newProperty = 10;
          });
      }
    );

    // Delete table property
    this.defineSchemaVersion(
      4,
      {
        users: '++id, name, email',
      },
      async trans => {
        await trans
          .table('users')
          .toCollection()
          .modify(user => {
            delete user.newProperty;
          });
      }
    );

    this.defineSchemaVersion(5, {
      documents: 'name, content',
    });
  }
}
