import { liveQuery, Table } from 'dexie';

export class BiaRepository<TEntity, TKey> {
  private table: Table<TEntity, TKey>;

  protected constructor(table: Table<TEntity, TKey>) {
    this.table = table;
  }

  items$ = liveQuery(() => this.read());

  async create(data: TEntity): Promise<TKey> {
    try {
      return await this.table.add(data);
    } catch (err) {
      console.error(`Error inserting into table: ${this.table.name}`, err);
      throw err;
    }
  }

  async read(filter?: (record: TEntity) => boolean): Promise<TEntity[]> {
    try {
      const allRecords = await this.table.toArray();
      return filter ? allRecords.filter(filter) : allRecords;
    } catch (err) {
      console.error(`Error reading from table: ${this.table.name}`, err);
      throw err;
    }
  }

  async update(id: TKey, changes: Partial<TEntity>): Promise<number> {
    try {
      return await this.table.update(id, changes);
    } catch (err) {
      console.error(`Error updating table: ${this.table.name}`, err);
      throw err;
    }
  }

  async delete(id: TKey): Promise<void> {
    try {
      await this.table.delete(id);
    } catch (err) {
      console.error(`Error deleting from table: ${this.table.name}`, err);
      throw err;
    }
  }
}
