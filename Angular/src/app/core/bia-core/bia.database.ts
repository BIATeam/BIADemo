import Dexie, { Transaction } from 'dexie';

export abstract class BiaDatabase extends Dexie {
  protected constructor(databaseName: string) {
    super(databaseName);
  }

  init() {
    this.defineSchemas();
  }

  protected abstract defineSchemas(): void;

  protected defineVersion(
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
        }
      });
    }
  }
}
