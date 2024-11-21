import { PortInfo } from '@serialport/bindings-interface';
import { SerialPort } from 'serialport';
import { ElectronCapacitorApp } from './setup';

export class SerialService {
  private ports: PortInfo[] = [];
  private listeningPorts: PortInfo[] = [];
  private isInit = false;

  constructor(private electronCapacitorApp: ElectronCapacitorApp) {}

  async init() {
    setInterval(() => {
      this.updatePorts();
    }, 1000);
  }

  private async updatePorts() {
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
          .webContents.send('serial-port-connected', newPort);
      }
      for (const oldPort of oldPorts) {
        if (this.listeningPorts.indexOf(oldPort) !== -1) {
          this.listeningPorts = this.listeningPorts.filter(
            p => p.path !== oldPort.path
          );
        }
        this.electronCapacitorApp
          .getMainWindow()
          .webContents.send('serial-port-disconnected', oldPort);
      }
    }

    this.ports = currentPorts;
    this.isInit = true;
  }

  async getPorts(): Promise<PortInfo[]> {
    return await SerialPort.list();
  }

  listen(portPath: any) {
    const portInfo = this.ports.find(p => p.path == portPath);

    if (this.listeningPorts.indexOf(portInfo) !== -1) {
      this.electronCapacitorApp
        .getMainWindow()
        .webContents.send(
          'serial-port-listening-error',
          portPath,
          new Error(`Already listening to port at path ${portPath}`)
        );
      return;
    }

    const port = new SerialPort({ path: portPath, baudRate: 9600 }, err => {
      if (err) {
        this.electronCapacitorApp
          .getMainWindow()
          .webContents.send('serial-port-listening-error', portPath, err);
      }
    });

    if (port) {
      port.on('data', data =>
        this.electronCapacitorApp
          .getMainWindow()
          .webContents.send('serial-port-data-received', portPath, data)
      );

      this.listeningPorts.push(portInfo);
    }
  }
}
