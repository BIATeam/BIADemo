export abstract class BiaOsBridge {
  usb: BiaOsBridgeUsb;
  database: BiaOsBridgeDatabase;
}

export abstract class BiaOsBridgeUsb {
  abstract getUsbPorts(): Promise<any[]>;
  abstract onUsbDeviceConnected(callback: (device: any) => void): void;
  abstract onUsbDeviceDisconnected(callback: (device: any) => void): void;
}

export abstract class BiaOsBridgeDatabase {
  abstract runQuery(query: string, params: any[]): Promise<number>;
  abstract getQuery<T>(query: string, params: any[]): Promise<T>;
}
