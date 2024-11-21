import { Injectable } from '@angular/core';
import {
  BiaOsBridge,
  BiaOsBridgeDatabase,
  BiaOsBridgeSerial,
  BiaOsBridgeUsb,
} from '../bia-os-bridge';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeCapacitor implements BiaOsBridge {
  serialPort: BiaOsBridgeSerial;
  usb: BiaOsBridgeUsb;
  database: BiaOsBridgeDatabase;
}
