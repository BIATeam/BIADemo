import { Device, usb } from 'usb';
import { ElectronCapacitorApp } from '../setup';

export class UsbService {
  constructor(private electronCapacitorApp: ElectronCapacitorApp) {}

  registerListeners() {
    usb.on('attach', device => {
      console.log('Device attached:', device);
      const deviceInfo = {
        vendorId: device.deviceDescriptor.idVendor,
        productId: device.deviceDescriptor.idProduct,
      };
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send('usb-device-connected', deviceInfo);
    });

    usb.on('detach', device => {
      console.log('Device detached:', device);
      const deviceInfo = {
        vendorId: device.deviceDescriptor.idVendor,
        productId: device.deviceDescriptor.idProduct,
      };
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send('usb-device-disconnected', deviceInfo);
    });
  }

  async getUsbPorts(): Promise<Device[]> {
    console.log('Request USB ports');
    try {
      return usb.getDeviceList();
    } catch (error) {
      throw new Error('Could not retrieve USB ports : ' + error);
    }
  }
}
