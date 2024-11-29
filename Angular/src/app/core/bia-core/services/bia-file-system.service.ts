import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BiaFileSystemService {
  private fileHandle: FileSystemFileHandle | null = null;

  /**
   * Opens a file picker to select a single file.
   */
  async openFilePicker(): Promise<File | null> {
    try {
      // Show file picker and allow selecting a single file.
      const [fileHandle] = await (window as any).showOpenFilePicker({
        multiple: false,
        types: [
          {
            description: 'All Files',
          },
        ],
      });
      this.fileHandle = fileHandle;

      // Get the file from the handle.
      const file = await fileHandle.getFile();
      return file;
    } catch (error) {
      console.error('Error while opening file picker:', error);
      return null;
    }
  }

  /**
   * Reads the content of the currently selected file.
   * @returns {Promise<string | null>} File content as string or null if no file is open.
   */
  async readFile(): Promise<string | null> {
    if (!this.fileHandle) {
      console.warn('No file is currently opened.');
      return null;
    }

    try {
      const file = await this.fileHandle.getFile();
      const content = await file.text();
      return content;
    } catch (error) {
      console.error('Error reading file:', error);
      return null;
    }
  }

  /**
   * Writes content to the currently selected file.
   * @param content The content to write to the file.
   */
  async writeFile(content: string): Promise<void> {
    if (!this.fileHandle) {
      console.warn('No file is currently opened.');
      return;
    }

    try {
      const writable = await this.fileHandle.createWritable();
      await writable.write(content);
      await writable.close();
      console.log('File written successfully.');
    } catch (error) {
      console.error('Error writing to file:', error);
    }
  }
}
