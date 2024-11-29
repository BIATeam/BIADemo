import { BiaUsbPlaformBridge } from '../bia.platform-bridge';

export class BiaUsbWebBridge implements BiaUsbPlaformBridge {
  private connectedDevices: Map<string, any> = new Map();

  constructor() {
    if (!('usb' in navigator)) {
      throw new Error('Web API usb is not supported in this browser');
    }
  }

  async getUsbPorts(): Promise<any[]> {
    try {
      // Request already connected USB devices
      const devices = await (navigator as any).usb.getDevices();
      return devices.map((device: any) => ({
        vendorId: device.vendorId,
        productId: device.productId,
        productName: device.productName,
        manufacturerName: device.manufacturerName,
      }));
    } catch (err) {
      console.error('Error fetching USB devices:', err);
      return [];
    }
  }

  onUsbDeviceConnected(callback: (device: any) => void): void {
    (navigator as any).usb.onconnect = (event: any) => {
      const device = event.device;
      this.connectedDevices.set(this.getDeviceKey(device), device); // Track the connected device
      callback({
        vendorId: device.vendorId,
        productId: device.productId,
        productName: device.productName,
        manufacturerName: device.manufacturerName,
      });
    };
  }

  onUsbDeviceDisconnected(callback: (device: any) => void): void {
    (navigator as any).usb.ondisconnect = (event: any) => {
      const device = event.device;
      const deviceKey = this.getDeviceKey(device);
      if (this.connectedDevices.has(deviceKey)) {
        this.connectedDevices.delete(deviceKey); // Remove the disconnected device
      }
      callback({
        vendorId: device.vendorId,
        productId: device.productId,
        productName: device.productName,
        manufacturerName: device.manufacturerName,
      });
    };
  }

  private getDeviceKey(device: any): string {
    return `${device.vendorId}-${device.productId}`;
  }
}
