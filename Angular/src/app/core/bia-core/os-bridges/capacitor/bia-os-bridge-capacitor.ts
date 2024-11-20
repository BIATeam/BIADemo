import { Injectable } from '@angular/core';
import {
  BiaOsBridge,
  BiaOsBridgeDatabase,
  BiaOsBridgeUsb,
} from '../bia-os-bridge';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeCapacitor implements BiaOsBridge {
  usb: BiaOsBridgeUsb;
  database: BiaOsBridgeDatabase;
}
