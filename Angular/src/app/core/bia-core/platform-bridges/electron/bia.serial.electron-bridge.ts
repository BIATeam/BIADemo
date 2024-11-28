import { BiaSerialPlatformBridge } from '../bia.platform-bridge';

export class BiaSerialElectronBridge implements BiaSerialPlatformBridge {
  async getSerialPorts(): Promise<any[]> {
    return await (
      window as any
    ).biaElectronBridge?.serialPort?.getSerialPorts();
  }
  onSerialPortConnected(callback: (portInfo: any) => void): void {
    (window as any).biaElectronBridge?.serialPort?.onSerialPortConnected(
      callback
    );
  }
  onSerialPortDisconnected(callback: (portInfo: any) => void): void {
    (window as any).biaElectronBridge?.serialPort?.onSerialPortDisconnected(
      callback
    );
  }
  listenPort(
    portPath: string,
    errorCallback: (portPath: string, err: any) => void,
    onDataReceivedCallback: (portPath: string, data: any) => void
  ): void {
    (window as any).biaElectronBridge?.serialPort?.listenPort(
      portPath,
      errorCallback,
      onDataReceivedCallback
    );
  }
}
