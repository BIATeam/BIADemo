import { PortInfo } from '@serialport/bindings-interface';
import { SerialPort } from 'serialport';
import { BiaIpc } from './bia.ipc';

export class SerialPortIpc extends BiaIpc {
  private ports: PortInfo[] = [];
  private listeningPorts: PortInfo[] = [];
  private isInit = false;

  override init(): void {
    super.init();

    setInterval(() => {
      this.updatePorts();
    }, 1000);
  }

  protected setHandling(): void {
    this.handle('serialPort:ports', this.getPorts);
    this.handle('serialPort:listen', (_event, portPath) =>
      this.listen(portPath)
    );
  }

  private async updatePorts(): Promise<void> {
    const currentPorts = await this.getPorts();

    const currentPortsMap = new Map(
      currentPorts.map(port => [port.path, port])
    );
    const previousPortsMap = new Map(this.ports.map(port => [port.path, port]));

    const newPorts = currentPorts.filter(
      port => !previousPortsMap.has(port.path)
    );
    const oldPorts = this.ports.filter(port => !currentPortsMap.has(port.path));

    if (this.isInit) {
      for (const newPort of newPorts) {
        this.electronCapacitorApp
          .getMainWindow()
          .webContents.send('serialPort:onConnected', newPort);
      }
      for (const oldPort of oldPorts) {
        if (this.listeningPorts.indexOf(oldPort) !== -1) {
          this.listeningPorts = this.listeningPorts.filter(
            p => p.path !== oldPort.path
          );
        }
        this.electronCapacitorApp
          .getMainWindow()
          .webContents.send('serialPort:onDisconnected', oldPort);
      }
    }

    this.ports = currentPorts;
    this.isInit = true;
  }

  private async getPorts(): Promise<PortInfo[]> {
    return await SerialPort.list();
  }

  private listen(portPath: any): void {
    const portInfo = this.ports.find(p => p.path == portPath);

    if (this.listeningPorts.indexOf(portInfo) !== -1) {
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send(
          'serialPort:listen:onError',
          portPath,
          new Error(`Already listening to port at path ${portPath}`)
        );
      return;
    }

    const port = new SerialPort({ path: portPath, baudRate: 9600 }, err => {
      if (err) {
        this.electronCapacitorApp
          .getMainWindow()
          .webContents.send('serialPort:listen:onError', portPath, err);
      }
    });

    if (port) {
      port.on('data', data =>
        this.electronCapacitorApp
          .getMainWindow()
          .webContents.send('serialPort:listen:onData', portPath, data)
      );

      this.listeningPorts.push(portInfo);
    }
  }
}
