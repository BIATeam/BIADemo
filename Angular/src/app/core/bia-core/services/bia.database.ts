import Dexie, { Table } from 'dexie';

interface Directory {
  key: string; // Unique identifier
  handle: FileSystemDirectoryHandle; // Handle to the directory
  metadata?: any; // Optional metadata (e.g., timestamps)
}

export class BiaDatabase extends Dexie {
  directories!: Table<Directory, string>;

  constructor() {
    super('BIADemoDatabase');
    this.version(1).stores({
      directories: 'key',
    });
  }

  async saveDirectory(
    key: string,
    handle: FileSystemDirectoryHandle,
    metadata?: any
  ): Promise<void> {
    await this.directories.put({ key, handle, metadata });
  }

  // Retrieve a directory handle by key
  async getDirectory(
    key: string
  ): Promise<FileSystemDirectoryHandle | undefined> {
    const record = await this.directories.get(key);
    return record?.handle;
  }
}

export const biaDatabase = new BiaDatabase();
