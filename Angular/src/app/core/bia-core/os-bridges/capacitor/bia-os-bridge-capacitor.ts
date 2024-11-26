import { Injectable } from '@angular/core';
import {
  BiaOsBridge,
  BiaOsBridgeDatabase,
  BiaOsBridgeSerial,
  BiaOsBridgeUsb,
} from '../bia-os-bridge';
import { BiaOsBridgeCapacitorDatabase } from './bia-os-bridge-capacitor-database';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeCapacitor implements BiaOsBridge {
  serialPort: BiaOsBridgeSerial;
  usb: BiaOsBridgeUsb;
  database: BiaOsBridgeDatabase = new BiaOsBridgeCapacitorDatabase();
}
