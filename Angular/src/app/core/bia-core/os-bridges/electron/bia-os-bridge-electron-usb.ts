import { BiaOsBridgeUsb } from '../bia-os-bridge';

export class BiaOsBridgeElectronUsb implements BiaOsBridgeUsb {
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