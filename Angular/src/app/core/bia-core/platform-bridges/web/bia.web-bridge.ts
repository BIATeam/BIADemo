import {
  BiaDatabasePlatformBridge,
  BiaPlatformBridge,
  BiaSerialPlatformBridge,
  BiaUsbPlaformBridge,
} from '../bia.platform-bridge';
import { BiaSerialWebBridge } from './bia.serial.web-bridge';
import { BiaUsbWebBridge } from './bia.usb.web-bridge';

export class BiaWebBridge implements BiaPlatformBridge {
  usb: BiaUsbPlaformBridge = new BiaUsbWebBridge();
  database: BiaDatabasePlatformBridge;
  serialPort: BiaSerialPlatformBridge = new BiaSerialWebBridge();
}
