import { PortInfo } from '@serialport/bindings-interface';
import { contextBridge, ipcRenderer } from 'electron';

require('./rt/electron-rt');

contextBridge.exposeInMainWorld('biaElectronBridge', {
  ipcRenderer: {
    invoke: (channel, ...args) => ipcRenderer.invoke(channel, ...args),
    on: (channel, listener) => ipcRenderer.on(channel, listener),
    removeListener: (channel, listener) =>
      ipcRenderer.removeListener(channel, listener),
  },
  usb: {
    getUsbPorts: () => ipcRenderer.invoke('usb:ports'),
    onUsbDeviceConnected: (callback: (device: any) => void) =>
      ipcRenderer.on('usb:onConnected', (_, device) => callback(device)),
    onUsbDeviceDisconnected: (callback: (device: any) => void) =>
      ipcRenderer.on('usb:onDisconnected', (_, device) => callback(device)),
  },
  database: {
    runQuery: (query: string, params: any[]) =>
      ipcRenderer.invoke('db:runQuery', query, params),
    getQuery: (query: string, params: any[]) =>
      ipcRenderer.invoke('db:getQuery', query, params),
  },
  serialPort: {
    getSerialPorts: () => ipcRenderer.invoke('serialPort:ports'),
    onSerialPortConnected: (callback: (portInfo: PortInfo) => void) =>
      ipcRenderer.on('serialPort:onConnected', (_, portInfo) =>
        callback(portInfo)
      ),
    onSerialPortDisconnected: (callback: (portInfo: PortInfo) => void) =>
      ipcRenderer.on('serialPort:onDisconnected', (_, portInfo) =>
        callback(portInfo)
      ),
    listenPort: (
      portPath: string,
      errorCallback: (portPath: string, err: any) => void,
      onDataReceivedCallback: (portPath: string, data: any) => void
    ) => {
      ipcRenderer.invoke('serialPort:listen', portPath);
      ipcRenderer.on('serialPort:listen:onError', (_, portPath, err) =>
        errorCallback(portPath, err)
      );
      ipcRenderer.on('serialPort:listen:onData', (_, portPath, data) =>
        onDataReceivedCallback(portPath, data)
      );
    },
  },
});
