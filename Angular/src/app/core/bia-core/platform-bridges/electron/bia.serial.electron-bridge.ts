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
    portInfo: any,
    errorCallback: (portInfo: any, err: any) => void,
    onDataReceivedCallback: (portInfo: any, data: any) => void
  ): void {
    (window as any).biaElectronBridge?.serialPort?.listenPort(
      portInfo.path,
      errorCallback,
      onDataReceivedCallback
    );
  }
}
