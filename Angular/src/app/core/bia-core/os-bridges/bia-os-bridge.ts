export abstract class BiaOsBridge {
  usb: BiaOsBridgeUsb;
}

export abstract class BiaOsBridgeUsb {
  abstract getUsbPorts(): Promise<any[]>;
  abstract onUsbDeviceConnected(callback: (device: any) => void): void;
  abstract onUsbDeviceDisconnected(callback: (device: any) => void): void;
}
