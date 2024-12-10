import Dexie, { Transaction } from 'dexie';

export abstract class BiaDatabase extends Dexie {
  private _isInit = false;
  private _isUpgradeFailure = false;

  protected constructor(databaseName: string) {
    super(databaseName);
  }

  init(): void {
    if (!this._isInit) {
      console.info(`Database ${this.name} init`);
      this._isUpgradeFailure = false;
      this.defineSchemas();
      this._isInit = !this._isUpgradeFailure;
    }
  }

  protected isInit(): boolean {
    return this._isInit;
  }

  protected isUpgradeFailure(): boolean {
    return this._isUpgradeFailure;
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
        try {
          await upgradeCallback(trans);
        } catch (err) {
          console.error(
            `Fail to upgrade database ${this.name} to version ${version}`,
            err
          );
          this._isUpgradeFailure = true;
        }
      });
    }
  }
}
