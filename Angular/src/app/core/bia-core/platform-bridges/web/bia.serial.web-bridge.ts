import { BiaSerialPlatformBridge } from '../bia.platform-bridge';

export class BiaSerialWebBridge implements BiaSerialPlatformBridge {
  constructor() {
    if (!('serial' in navigator)) {
      throw new Error('Web API serial is not supported in this browser');
    }
  }

  async getSerialPorts(): Promise<any[]> {
    try {
      // Get the list of all connected serial ports
      const ports = await (navigator as any).serial.getPorts();
      return ports;
    } catch (err) {
      console.error('Error fetching serial ports:', err);
      return [];
    }
  }

  onSerialPortConnected(callback: (portInfo: any) => void): void {
    (navigator as any).serial.addEventListener('connect', (event: any) => {
      callback(event.target.getInfo());
    });
  }

  onSerialPortDisconnected(callback: (portInfo: any) => void): void {
    (navigator as any).serial.addEventListener('disconnect', (event: any) => {
      callback(event.target.getInfo());
    });
  }

  async listenPort(
    portInfo: any,
    errorCallback: (portInfo: any, err: any) => void,
    onDataReceivedCallback: (portInfo: any, data: any) => void
  ): Promise<void> {
    console.info('Listen port', portInfo);
    const ports = await this.getSerialPorts();
    const port = ports.find((p: any) => {
      const infos = p.getInfo();
      if (
        infos.usbProductId === portInfo.usbProductId &&
        infos.usbVendorId === portInfo.usbVendorId
      ) {
        return p;
      }
    });

    if (!port) {
      throw new Error(`Port not found`);
    }

    try {
      await port.open({ baudRate: 9600 });
      await port.setSignals({ dataTerminalReady: true, requestToSend: true });
      while (port.readable) {
        const reader = port.readable.getReader();
        try {
          // eslint-disable-next-line no-constant-condition
          while (true) {
            const { value, done } = await reader.read();
            if (done) {
              console.log('End of port read');
              break;
            }
            onDataReceivedCallback(port, value);
          }
        } catch (error) {
          console.warn(error);
        } finally {
          reader.releaseLock();
        }
      }
    } catch (err) {
      console.error('Error listening to port:', err);
      errorCallback(port, err);
    }
  }
}
