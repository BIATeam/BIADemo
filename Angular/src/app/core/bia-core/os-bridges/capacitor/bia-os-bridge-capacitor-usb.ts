import { BiaOsBridgeUsb } from '../bia-os-bridge';

export class BiaOsBridgeCapacitorUsb implements BiaOsBridgeUsb {
  onUsbDeviceConnected(callback: (device: any) => void): void {
    throw new Error('Method not implemented.');
  }

  onUsbDeviceDisconnected(callback: (device: any) => void): void {
    throw new Error('Method not implemented.');
  }

  getUsbPorts(): Promise<any[]> {
    throw new Error('Method not implemented.');
  }
}
