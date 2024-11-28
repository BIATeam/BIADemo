import { Injectable } from '@angular/core';
import {
  BiaDatabasePlatformBridge,
  BiaPlatformBridge,
  BiaSerialPlatformBridge,
  BiaUsbPlaformBridge,
} from '../bia.platform-bridge';
import { BiaDatabaseCapacitorBridge } from './bia.database.capacitor-bridge';

@Injectable({
  providedIn: 'root',
})
export class BiaCapacitorBridge implements BiaPlatformBridge {
  serialPort: BiaSerialPlatformBridge;
  usb: BiaUsbPlaformBridge;
  database: BiaDatabasePlatformBridge = new BiaDatabaseCapacitorBridge();
}
