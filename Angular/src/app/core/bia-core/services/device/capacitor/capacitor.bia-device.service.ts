import { Injectable } from '@angular/core';
import { BiaDeviceService } from '../bia-device.service';

@Injectable({
  providedIn: 'root',
})
export class CapacitorBiaDeviceService implements BiaDeviceService {
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
