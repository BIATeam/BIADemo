import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BiaDirectorySystemService {
  private directoryHandle: FileSystemDirectoryHandle | null = null;

  /**
   * Opens a directory picker to allow the user to select a folder.
   */
  async openDirectoryPicker(): Promise<FileSystemDirectoryHandle | null> {
    try {
      this.directoryHandle = await (window as any).showDirectoryPicker();
      console.log('Directory selected:', this.directoryHandle?.name);
      return this.directoryHandle;
    } catch (error) {
      console.error('Error while opening directory picker:', error);
      return null;
    }
  }

  setHandler(handler: FileSystemDirectoryHandle) {
    this.directoryHandle = handler;
  }

  /**
   * Adds a new file to the selected directory.
   * @param fileName The name of the new file.
   * @param content The content to write to the file.
   */
  async addFile(fileName: string, content: string): Promise<void> {
    if (!this.directoryHandle) {
      console.warn('No directory is currently opened.');
      return;
    }

    try {
      // Create a new file handle or overwrite if it already exists.
      const fileHandle = await this.directoryHandle.getFileHandle(fileName, {
        create: true,
      });
      const writable = await fileHandle.createWritable();
      await writable.write(content);
      await writable.close();
      console.log(`File "${fileName}" created successfully.`);
    } catch (error) {
      console.error('Error creating file:', error);
    }
  }

  /**
   * Deletes a file or directory from the selected directory.
   * @param entryName The name of the file or directory to delete.
   */
  async deleteEntry(entryName: string): Promise<void> {
    if (!this.directoryHandle) {
      console.warn('No directory is currently opened.');
      return;
    }

    try {
      // Remove the specified file or directory.
      await this.directoryHandle.removeEntry(entryName, { recursive: true });
      console.log(`Entry "${entryName}" deleted successfully.`);
    } catch (error) {
      console.error('Error deleting entry:', error);
    }
  }

  /**
   * Reads all files in the selected directory, including subdirectories (recursively).
   */
  async readDirectoryContents(): Promise<string[]> {
    if (!this.directoryHandle) {
      console.warn('No directory is currently opened.');
      return [];
    }

    const fileNames: string[] = [];
    await this.processDirectory(this.directoryHandle, '', fileNames);
    return fileNames;
  }

  /**
   * Processes a directory recursively to collect file names.
   * @param dirHandle The directory handle.
   * @param path Current path inside the directory tree.
   * @param fileNames The array to collect file names.
   */
  private async processDirectory(
    dirHandle: FileSystemDirectoryHandle,
    path: string,
    fileNames: string[]
  ) {
    for await (const [name, handle] of dirHandle as any) {
      const currentPath = `${path}/${name}`;
      if (handle.kind === 'file') {
        fileNames.push(currentPath);
      } else if (handle.kind === 'directory') {
        await this.processDirectory(
          handle as FileSystemDirectoryHandle,
          currentPath,
          fileNames
        );
      }
    }
  }
}
