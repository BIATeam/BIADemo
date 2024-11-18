import { Injectable } from '@angular/core';
import { BiaOsBridge } from '../bia-os-bridge';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeElectron implements BiaOsBridge {
  onUsbDeviceConnected(callback: (device: any) => void): void {
    (window as any).electronBridge?.usb?.onUsbDeviceConnected(callback);
  }

  onUsbDeviceDisconnected(callback: (device: any) => void): void {
    (window as any).electronBridge?.usb?.onUsbDeviceDisconnected(callback);
  }

  async getUsbPorts(): Promise<any[]> {
    try {
      const ports = await (window as any).electronBridge?.usb?.getUsbPorts();
      return ports;
    } catch (error) {
      console.error('Failed to retrieve USB ports:', error);
      return [];
    }
  }
}
