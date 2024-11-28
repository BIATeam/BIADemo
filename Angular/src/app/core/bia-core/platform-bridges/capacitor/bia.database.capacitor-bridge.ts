import { SQLiteDBConnection } from '@capacitor-community/sqlite';
import { BiaDatabasePlatformBridge } from '../bia.platform-bridge';
import { CapacitorSqliteService } from './capacitor.sqlite.service';

export class BiaDatabaseCapacitorBridge extends BiaDatabasePlatformBridge {
  private sqliteService: CapacitorSqliteService = new CapacitorSqliteService();
  private sqliteDb: SQLiteDBConnection;
  private isInit = false;

  async init() {
    await this.sqliteService.initializePlugin();
    this.sqliteDb = await this.sqliteService.openDatabase(
      'biaDemo',
      false,
      'no-encryption',
      1,
      false
    );
  }

  async runQuery(query: string, params: any[]): Promise<number> {
    if (!this.isInit) {
      await this.init();
    }

    const res = await this.sqliteDb.run(query, params);
    console.log('RunQuery', JSON.stringify(res));
    return res.changes?.changes ?? 0;
  }

  async getQuery<T>(query: string, params: any[]): Promise<T> {
    if (!this.isInit) {
      await this.init();
    }

    const res = await this.sqliteDb.query(query, params);
    console.log('GetQuery', JSON.stringify(res));
    return res.values as T;
  }
}
