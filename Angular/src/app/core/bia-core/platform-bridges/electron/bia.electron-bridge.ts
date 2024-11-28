import { Injectable } from '@angular/core';
import {
  BiaDatabasePlatformBridge,
  BiaPlatformBridge,
  BiaSerialPlatformBridge,
  BiaUsbPlaformBridge,
} from '../bia.platform-bridge';
import { BiaDatabaseElectronBridge } from './bia.database.electron-bridge';
import { BiaSerialElectronBridge } from './bia.serial.electron-bridge';
import { BiaUsbElectronBridge } from './bia.usb.electron-bridge';

@Injectable({
  providedIn: 'root',
})
export class BiaElectronBridge implements BiaPlatformBridge {
  usb: BiaUsbPlaformBridge = new BiaUsbElectronBridge();
  database: BiaDatabasePlatformBridge = new BiaDatabaseElectronBridge();
  serialPort: BiaSerialPlatformBridge = new BiaSerialElectronBridge();
}
