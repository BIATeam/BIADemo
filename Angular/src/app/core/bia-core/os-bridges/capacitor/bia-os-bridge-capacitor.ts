import { Injectable } from '@angular/core';
import { BiaOsBridge, BiaOsBridgeUsb } from '../bia-os-bridge';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeCapacitor implements BiaOsBridge {
  usb: BiaOsBridgeUsb = new BiaOsBridgeCapacitorUsb();
}

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
