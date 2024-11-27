import { ipcMain } from 'electron';
import sqlite3 from 'sqlite3';
import { ElectronCapacitorApp } from '../setup';
import { BiaIpc } from './bia.ipc';

export class DatabaseIpc extends BiaIpc {
  dbPath: string;
  private db: sqlite3.Database;

  constructor(dbPath: string, electronCapacitorApp: ElectronCapacitorApp) {
    super(electronCapacitorApp);
    this.dbPath = dbPath;
  }

  override init(): void {
    super.init();

    this.db = new sqlite3.Database(this.dbPath);
  }

  protected setHandling(): void {
    ipcMain.handle('db:runQuery', async (_event, query, params) => {
      try {
        const result = await this.runQuery(query, params);
        return result;
      } catch (error) {
        console.error('Error running query:', error);
        throw error;
      }
    });

    ipcMain.handle('db:getQuery', async (_event, query, params) => {
      try {
        const result = await this.getQuery(query, params);
        return result;
      } catch (error) {
        console.error('Error fetching data:', error);
        throw error;
      }
    });
  }

  private runQuery(query, params = []) {
    return new Promise((resolve, reject) => {
      this.db.run(query, params, function (err) {
        if (err) {
          reject(err);
        } else {
          resolve(this.changes);
        }
      });
    });
  }

  private getQuery(query, params = []) {
    return new Promise((resolve, reject) => {
      this.db.all(query, params, (err, rows) => {
        if (err) {
          reject(err);
        } else {
          resolve(rows);
        }
      });
    });
  }
}
