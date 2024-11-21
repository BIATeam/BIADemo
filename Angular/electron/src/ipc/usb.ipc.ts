import { Device, usb } from 'usb';
import { BiaIpc } from './bia.ipc';

export class UsbIpc extends BiaIpc {
  protected setHandling(): void {
    this.handle('usb:ports', this.getUsbPorts);

    usb.on('attach', async device => {
      this.send('usb:onConnected', device);
    });

    usb.on('detach', async device => {
      this.send('usb:onDisconnected', device);
    });
  }

  private async getUsbPorts(): Promise<Device[]> {
    return usb.getDeviceList();
  }
}
