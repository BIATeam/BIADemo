import Dexie, { Transaction } from 'dexie';

export abstract class BiaDatabase extends Dexie {
  private _isInit = false;
  private _isUpgradeFailure = false;

  protected constructor(databaseName: string) {
    super(databaseName);
  }

  protected isInit(): boolean {
    return this._isInit;
  }

  protected isUpgradeFailure(): boolean {
    return this._isUpgradeFailure;
  }

  init(): void {
    if (!this._isInit) {
      this._isUpgradeFailure = false;
      this.defineSchemas();
      this._isInit = !this._isUpgradeFailure;
    }
  }

  protected abstract defineSchemas(): void;

  protected defineSchemaVersion(
    version: number,
    schema: any,
    upgradeCallback?: (trans: Transaction) => Promise<void>
  ) {
    const versionBuilder = this.version(version).stores(schema);
    if (upgradeCallback) {
      versionBuilder.upgrade(async trans => {
        console.info(`Migrate database ${this.name} to version ${version}...`);
        try {
          await upgradeCallback(trans);
          console.info(
            `Database ${this.name} has migrated successfully to version ${version}`
          );
        } catch (err) {
          console.error(
            `Fail to migrate database ${this.name} to version ${version}`,
            err
          );
          this._isUpgradeFailure = true;
        }
      });
    }
  }
}
