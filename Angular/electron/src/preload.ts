import { contextBridge, ipcRenderer } from 'electron';

require('./rt/electron-rt');
//////////////////////////////
// User Defined Preload scripts below
console.log('User Preload!');

contextBridge.exposeInMainWorld('electron', {
  ipcRenderer: {
    invoke: (channel, ...args) => ipcRenderer.invoke(channel, ...args),
    on: (channel, listener) => ipcRenderer.on(channel, listener),
    removeListener: (channel, listener) =>
      ipcRenderer.removeListener(channel, listener),
  },
  bia: {
    getUsbPorts: () => ipcRenderer.invoke('get-usb-ports'),
    onUsbDeviceConnected: (callback: (device: any) => void) =>
      ipcRenderer.on('usb-device-connected', (_, device) => callback(device)),
    onUsbDeviceDisconnected: (callback: (device: any) => void) =>
      ipcRenderer.on('usb-device-disconnected', (_, device) =>
        callback(device)
      ),
  },
});
