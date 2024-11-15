import { Injectable } from '@angular/core';
import { BiaDeviceService } from '../bia-device.service';

@Injectable({
  providedIn: 'root',
})
export class ElectronBiaDeviceService implements BiaDeviceService {
  async getUsbPorts(): Promise<string[]> {
    try {
      const ports = await (window as any).electron?.ipcRenderer.invoke(
        'get-usb-ports'
      );
      return ports;
    } catch (error) {
      console.error('Failed to retrieve USB ports:', error);
      return [];
    }
  }
}
