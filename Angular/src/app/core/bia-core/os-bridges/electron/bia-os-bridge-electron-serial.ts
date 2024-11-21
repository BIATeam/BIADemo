import { BiaOsBridgeSerial } from '../bia-os-bridge';

export class BiaOsBridgeElectronSerial implements BiaOsBridgeSerial {
  async getSerialPorts(): Promise<any[]> {
    return await (window as any).electronBridge?.serialPort?.getSerialPorts();
  }
  onSerialPortConnected(callback: (portInfo: any) => void): void {
    (window as any).electronBridge?.serialPort?.onSerialPortConnected(callback);
  }
  onSerialPortDisconnected(callback: (portInfo: any) => void): void {
    (window as any).electronBridge?.serialPort?.onSerialPortDisconnected(
      callback
    );
  }
  listenPort(
    portPath: string,
    errorCallback: (portPath: string, err: any) => void,
    onDataReceivedCallback: (portPath: string, data: any) => void
  ): void {
    (window as any).electronBridge?.serialPort?.listenPort(
      portPath,
      errorCallback,
      onDataReceivedCallback
    );
  }
}
