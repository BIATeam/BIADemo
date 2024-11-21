import log from 'electron-log';
import { SerialPort } from 'serialport';
import { Device, usb } from 'usb';
import { ElectronCapacitorApp } from '../setup';

export class UsbService {
  constructor(private electronCapacitorApp: ElectronCapacitorApp) {}

  registerListeners() {
    usb.on('attach', async device => {
      log.log('Device attached:', device);
      const deviceInfo = {
        vendorId: device.deviceDescriptor.idVendor,
        productId: device.deviceDescriptor.idProduct,
      };
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send('usb-device-connected', deviceInfo);
      await this.listenLastSerialPort();
    });

    usb.on('detach', async device => {
      log.log('Device detached:', device);
      const deviceInfo = {
        vendorId: device.deviceDescriptor.idVendor,
        productId: device.deviceDescriptor.idProduct,
      };
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send('usb-device-disconnected', deviceInfo);
      await this.listenLastSerialPort();
    });
  }

  async getUsbPorts(): Promise<Device[]> {
    log.log('Request USB ports');
    try {
      return usb.getDeviceList();
    } catch (error) {
      throw new Error('Could not retrieve USB ports : ' + error);
    }
  }

  async listenLastSerialPort() {
    const ports = await SerialPort.list();
    log.log('Ports', ports);
    if (ports.length === 0) {
      return;
    }

    const targetPort = ports[ports.length - 1];
    if (targetPort) {
      log.log('TargetPort', targetPort);
      const serialPort = new SerialPort(
        { path: targetPort.path, baudRate: 9600 },
        err => {
          if (err) log.log('Serial port error', err);
          else log.log('Target port connected');
        }
      );

      if (serialPort) {
        serialPort.on('data', data => {
          log.log('Data received', data);
        });
      }
    }
  }
}
