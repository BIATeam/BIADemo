import { Injectable } from '@angular/core';
import { BiaOsBridge, BiaOsBridgeUsb } from '../bia-os-bridge';
import { BiaOsBridgeCapacitorUsb } from './bia-os-bridge-capacitor-usb';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeCapacitor implements BiaOsBridge {
  usb: BiaOsBridgeUsb = new BiaOsBridgeCapacitorUsb();
}
