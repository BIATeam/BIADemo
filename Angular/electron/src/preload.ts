import { PortInfo } from '@serialport/bindings-interface';
import { contextBridge, ipcRenderer } from 'electron';

require('./rt/electron-rt');
//////////////////////////////
// User Defined Preload scripts below
console.log('User Preload!');

ipcRenderer.invoke('try-dotnet-interop');

contextBridge.exposeInMainWorld('electronBridge', {
  ipcRenderer: {
    invoke: (channel, ...args) => ipcRenderer.invoke(channel, ...args),
    on: (channel, listener) => ipcRenderer.on(channel, listener),
    removeListener: (channel, listener) =>
      ipcRenderer.removeListener(channel, listener),
  },
  usb: {
    getUsbPorts: () => ipcRenderer.invoke('get-usb-ports'),
    onUsbDeviceConnected: (callback: (device: any) => void) =>
      ipcRenderer.on('usb-device-connected', (_, device) => callback(device)),
    onUsbDeviceDisconnected: (callback: (device: any) => void) =>
      ipcRenderer.on('usb-device-disconnected', (_, device) =>
        callback(device)
      ),
  },
  database: {
    runQuery: (query: string, params: any[]) =>
      ipcRenderer.invoke('db:run', query, params),
    getQuery: (query: string, params: any[]) =>
      ipcRenderer.invoke('db:get', query, params),
  },
  serialPort: {
    getSerialPorts: () => ipcRenderer.invoke('get-serial-ports'),
    onSerialPortConnected: (callback: (portInfo: PortInfo) => void) =>
      ipcRenderer.on('serial-port-connected', (_, portInfo) =>
        callback(portInfo)
      ),
    onSerialPortDisconnected: (callback: (portInfo: PortInfo) => void) =>
      ipcRenderer.on('serial-port-disconnected', (_, portInfo) =>
        callback(portInfo)
      ),
    listenPort: (
      portPath: string,
      errorCallback: (portPath: string, err: any) => void,
      onDataReceivedCallback: (portPath: string, data: any) => void
    ) => {
      ipcRenderer.invoke('listen-port', portPath);
      ipcRenderer.on('serial-port-listening-error', (_, portPath, err) =>
        errorCallback(portPath, err)
      );
      ipcRenderer.on('serial-port-data-received', (_, portPath, data) =>
        onDataReceivedCallback(portPath, data)
      );
    },
  },
});
