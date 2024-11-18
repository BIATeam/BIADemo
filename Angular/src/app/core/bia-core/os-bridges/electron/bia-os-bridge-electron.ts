import { Injectable } from '@angular/core';
import { BiaOsBridge, BiaOsBridgeUsb } from '../bia-os-bridge';
import { BiaOsBridgeElectronUsb } from './bia-os-bridge-electron-usb';

@Injectable({
  providedIn: 'root',
})
export class BiaOsBridgeElectron implements BiaOsBridge {
  usb: BiaOsBridgeUsb = new BiaOsBridgeElectronUsb();
}
