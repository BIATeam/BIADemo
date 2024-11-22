import { Capacitor } from '@capacitor/core';
import { BiaOsBridge } from './bia-os-bridge';
import { BiaOsBridgeCapacitor } from './capacitor/bia-os-bridge-capacitor';
import { BiaOsBridgeElectron } from './electron/bia-os-bridge-electron';

export function biaOsBridgeFactory(): BiaOsBridge {
  console.log('Request BiaOsBridge');

  const osPlatform = Capacitor.getPlatform();
  if (osPlatform === 'electron') {
    console.log('Electron BisOSBridge');
    return new BiaOsBridgeElectron();
  }

  if (osPlatform === 'android' || osPlatform === 'ios') {
    console.log('Capacitor BisOSBridge');
    return new BiaOsBridgeCapacitor();
  }

  throw new Error(`No compatible BIA OS Bridge for the platform ${osPlatform}`);
}
