import { BiaUsbPlaformBridge } from '../bia.platform-bridge';

export class BiaUsbElectronBridge implements BiaUsbPlaformBridge {
  onUsbDeviceConnected(callback: (device: any) => void): void {
    (window as any).biaElectronBridge?.usb?.onUsbDeviceConnected(callback);
  }

  onUsbDeviceDisconnected(callback: (device: any) => void): void {
    (window as any).biaElectronBridge?.usb?.onUsbDeviceDisconnected(callback);
  }

  async getUsbPorts(): Promise<any[]> {
    try {
      const ports = await (window as any).biaElectronBridge?.usb?.getUsbPorts();
      return ports;
    } catch (error) {
      console.error('Failed to retrieve USB ports:', error);
      return [];
    }
  }
}
