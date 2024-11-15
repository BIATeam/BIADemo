export abstract class BiaDeviceService {
  abstract getUsbPorts(): Promise<string[]>;
}
