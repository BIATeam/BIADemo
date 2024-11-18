import { Injectable } from '@angular/core';
import { BiaDeviceService } from '../bia-device.service';

@Injectable({
  providedIn: 'root',
})
export class ElectronBiaDeviceService implements BiaDeviceService {
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
