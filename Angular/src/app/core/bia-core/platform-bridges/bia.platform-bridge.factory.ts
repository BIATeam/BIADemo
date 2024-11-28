import { Capacitor } from '@capacitor/core';
import { BiaPlatformBridge } from './bia.platform-bridge';
import { BiaCapacitorBridge } from './capacitor/bia.capacitor-bridge';
import { BiaElectronBridge } from './electron/bia.electron-bridge';
import { BiaWebBridge } from './web/bia.web-bridge';

export function biaPlatformBridgeFactory(): BiaPlatformBridge {
  console.log('Request BiaPlatformBridge');

  const osPlatform = Capacitor.getPlatform();
  if (osPlatform === 'electron') {
    console.log('Electron BiaPlatformBridge');
    return new BiaElectronBridge();
  }

  if (osPlatform === 'android' || osPlatform === 'ios') {
    console.log('Capacitor BiaPlatformBridge');
    return new BiaCapacitorBridge();
  }

  if (osPlatform === 'web') {
    console.log('Web BiaPlatformBridge');
    return new BiaWebBridge();
  }

  throw new Error(`No compatible BIA OS Bridge for the platform ${osPlatform}`);
}
