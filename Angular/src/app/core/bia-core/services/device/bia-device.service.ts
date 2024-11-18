export abstract class BiaDeviceService {
  abstract getUsbPorts(): Promise<any[]>;
  abstract onUsbDeviceConnected(callback: (device: any) => void): void;
  abstract onUsbDeviceDisconnected(callback: (device: any) => void): void;
}
