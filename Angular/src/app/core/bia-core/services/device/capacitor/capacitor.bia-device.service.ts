import { Injectable } from '@angular/core';
import { BiaDeviceService } from '../bia-device.service';

@Injectable({
  providedIn: 'root',
})
export class CapacitorBiaDeviceService implements BiaDeviceService {
  getUsbPorts(): Promise<any[]> {
    throw new Error('Method not implemented.');
  }
}
