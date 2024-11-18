import { BiaOsBridge } from './bia-os-bridge';
import { BiaOsBridgeCapacitor } from './capacitor/bia-os-bridge-capacitor';
import { BiaOsBridgeElectron } from './electron/bia-os-bridge-electron';

export function biaOsBridgeFactory(): BiaOsBridge {
  console.log('Request BiaOsBridge', window);

  if ((window as any).electronBridge) {
    return new BiaOsBridgeElectron();
  }

  if ((window as any).capacitor) {
    return new BiaOsBridgeCapacitor();
  }

  throw new Error('No compatible BIA OS Bridge for your platform');
}
