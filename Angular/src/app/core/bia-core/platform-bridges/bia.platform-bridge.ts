export abstract class BiaPlatformBridge {
  usb: BiaUsbPlaformBridge;
  database: BiaDatabasePlatformBridge;
  serialPort: BiaSerialPlatformBridge;
}

export abstract class BiaUsbPlaformBridge {
  abstract getUsbPorts(): Promise<any[]>;
  abstract onUsbDeviceConnected(callback: (device: any) => void): void;
  abstract onUsbDeviceDisconnected(callback: (device: any) => void): void;
}

export abstract class BiaDatabasePlatformBridge {
  abstract runQuery(query: string, params: any[]): Promise<number>;
  abstract getQuery<T>(query: string, params: any[]): Promise<T>;
}

export abstract class BiaSerialPlatformBridge {
  abstract getSerialPorts(): Promise<any[]>;
  abstract onSerialPortConnected(callback: (portInfo: any) => void): void;
  abstract onSerialPortDisconnected(callback: (portInfo: any) => void): void;
  abstract listenPort(
    portPath: string,
    errorCallback: (portPath: string, err: any) => void,
    onDataReceivedCallback: (portPath: string, data: any) => void
  ): void;
}
