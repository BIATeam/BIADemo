import { Provider } from '@angular/core';
import { BiaOsBridge } from './bia-os-bridge';
import { BiaOsBridgeCapacitor } from './capacitor/bia-os-bridge-capacitor';
import { BiaOsBridgeElectron } from './electron/bia-os-bridge-electron';

export function biaOsBridgeFactory(): BiaOsBridge {
  console.log('Request BiaDeviceService', window);

  if ((window as any).electron) {
    return new BiaOsBridgeElectron();
  }

  if ((window as any).capacitor) {
    return new BiaOsBridgeCapacitor();
  }

  throw new Error('No compatible BiaDeviceService for your platform');
}

export const biaOsBridgeProvider: Provider = {
  provide: BiaOsBridge,
  useFactory: biaOsBridgeFactory,
};
