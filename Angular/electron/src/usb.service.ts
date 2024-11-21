import { Device, usb } from 'usb';
import { ElectronCapacitorApp } from './setup';

export class UsbService {
  constructor(private electronCapacitorApp: ElectronCapacitorApp) {}

  init() {
    usb.on('attach', async device => {
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send('usb-device-connected', device);
    });

    usb.on('detach', async device => {
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send('usb-device-disconnected', device);
    });
  }

  async getUsbPorts(): Promise<Device[]> {
    try {
      return usb.getDeviceList();
    } catch (error) {
      throw new Error('Could not retrieve USB ports : ' + error);
    }
  }
}
