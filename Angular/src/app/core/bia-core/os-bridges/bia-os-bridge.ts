export abstract class BiaOsBridge {
  abstract getUsbPorts(): Promise<any[]>;
  abstract onUsbDeviceConnected(callback: (device: any) => void): void;
  abstract onUsbDeviceDisconnected(callback: (device: any) => void): void;
}
