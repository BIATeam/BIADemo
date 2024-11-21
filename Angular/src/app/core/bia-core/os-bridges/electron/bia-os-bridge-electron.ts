import { Injectable } from '@angular/core';
import {
  BiaOsBridge,
  BiaOsBridgeDatabase,
  BiaOsBridgeSerial,
  BiaOsBridgeUsb,
} from '../bia-os-bridge';
import { BiaOsBridgeElectronDatabase } from './bia-os-bridge-electron-database';
import { BiaOsBridgeElectronSerial } from './bia-os-bridge-electron-serial';
import { BiaOsBridgeElectronUsb } from './bia-os-bridge-electron-usb';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeElectron implements BiaOsBridge {
  usb: BiaOsBridgeUsb = new BiaOsBridgeElectronUsb();
  database: BiaOsBridgeDatabase = new BiaOsBridgeElectronDatabase();
  serialPort: BiaOsBridgeSerial = new BiaOsBridgeElectronSerial();
}
