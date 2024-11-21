import { ipcMain } from 'electron';
import { ElectronCapacitorApp } from '../setup';

export abstract class BiaIpc {
  constructor(protected electronCapacitorApp: ElectronCapacitorApp) {}

  init(): void {
    this.setHandling();
  }

  protected abstract setHandling(): void;

  protected send(channel: string, ...args: any[]): void {
    this.electronCapacitorApp.getMainWindow().webContents.send(channel, args);
  }

  protected handle(
    channel: string,
    listener: (
      event: Electron.IpcMainInvokeEvent,
      ...args: any[]
    ) => Promise<any> | any
  ): void {
    ipcMain.handle(channel, listener);
  }
}
