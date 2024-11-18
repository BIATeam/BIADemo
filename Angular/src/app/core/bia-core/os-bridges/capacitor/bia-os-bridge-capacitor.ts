import { Injectable } from '@angular/core';
import { BiaOsBridge } from '../bia-os-bridge';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeCapacitor implements BiaOsBridge {
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
