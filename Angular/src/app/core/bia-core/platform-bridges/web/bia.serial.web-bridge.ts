import { BiaSerialPlatformBridge } from '../bia.platform-bridge';

export class BiaSerialWebBridge implements BiaSerialPlatformBridge {
  constructor() {
    if (!('serial' in navigator)) {
      throw new Error('Web API serial is not supported in this browser');
    }
  }

  private connectedPorts: Map<string, any> = new Map();

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
      console.log('Serial connect', event);
      const port = event.target;
      const portInfo = port.getInfo(); // Port information
      this.connectedPorts.set(JSON.stringify(portInfo), port); // Track the connected port
      callback(portInfo);
    });
  }

  onSerialPortDisconnected(callback: (portInfo: any) => void): void {
    (navigator as any).serial.addEventListener('disconnect', (event: any) => {
      const port = event.target;
      const portInfo = port.getInfo(); // Port information
      this.connectedPorts.delete(JSON.stringify(portInfo)); // Remove the port from tracking
      callback(portInfo);
    });
  }

  async listenPort(
    portPath: string,
    errorCallback: (portPath: string, err: any) => void,
    onDataReceivedCallback: (portPath: string, data: any) => void
  ): Promise<void> {
    try {
      const port = Array.from(this.connectedPorts.values()).find(
        p => JSON.stringify(p.getInfo()) === portPath
      );
      if (!port) {
        throw new Error(`Port with path ${portPath} not found`);
      }

      await port.open({ baudRate: 9600 });

      const decoder = new TextDecoderStream();
      const inputStream = port.readable.pipeThrough(decoder);
      const reader = inputStream.getReader();

      // Start listening for data
      const readLoop = async () => {
        try {
          setInterval(async () => {
            const { value, done } = await reader.read();
            if (done) {
              return;
            }
            if (value) {
              onDataReceivedCallback(portPath, value);
            }
          }, 100);
        } catch (err) {
          console.error('Error reading from port:', err);
          errorCallback(portPath, err);
        }
      };

      readLoop();
    } catch (err) {
      console.error('Error listening to port:', err);
      errorCallback(portPath, err);
    }
  }
}
