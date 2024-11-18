import { Injectable } from '@angular/core';
import { BiaDeviceService } from '../bia-device.service';

@Injectable({
  providedIn: 'root',
})
export class ElectronBiaDeviceService implements BiaDeviceService {
  onUsbDeviceConnected(callback: (device: any) => void): void {
    (window as any).electron?.bia?.onUsbDeviceConnected(callback);
  }

  onUsbDeviceDisconnected(callback: (device: any) => void): void {
    (window as any).electron?.bia?.onUsbDeviceDisconnected(callback);
  }

  async getUsbPorts(): Promise<any[]> {
    try {
      const ports = await (window as any).electron?.bia?.getUsbPorts();
      return ports;
    } catch (error) {
      console.error('Failed to retrieve USB ports:', error);
      return [];
    }
  }
}
