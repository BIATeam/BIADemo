import { Provider } from '@angular/core';
import { BiaDeviceService } from './bia-device.service';
import { CapacitorBiaDeviceService } from './capacitor/capacitor.bia-device.service';
import { ElectronBiaDeviceService } from './electron/electron.bia-device.service';

export function biaDeviceServiceFactory(): BiaDeviceService {
  console.log('Request BiaDeviceService', window);

  if ((window as any).electron) {
    return new ElectronBiaDeviceService();
  }

  if ((window as any).capacitor) {
    return new CapacitorBiaDeviceService();
  }

  throw new Error('No compatible BiaDeviceService for your platform');
}

export const biaDeviceServiceProvider: Provider = {
  provide: BiaDeviceService,
  useFactory: biaDeviceServiceFactory,
};
