import { Component, OnDestroy, OnInit } from '@angular/core';
import { BiaPlatformBridge } from 'src/app/core/bia-core/platform-bridges/bia.platform-bridge';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  constructor(
    private layoutService: BiaLayoutService,
    private platformBridge: BiaPlatformBridge
  ) {}
  async ngOnInit(): Promise<void> {
    this.layoutService.hideBreadcrumb();

    //  === Platform Bridge Database ===
    // await this.platformBridge.database.runQuery(
    //   `
    //     CREATE TABLE IF NOT EXISTS users (
    //         id INTEGER PRIMARY KEY AUTOINCREMENT,
    //         name TEXT NOT NULL,
    //         email TEXT NOT NULL UNIQUE
    //     );
    // `,
    //   []
    // );

    // const addUsers = await this.platformBridge.database.runQuery(
    //   'INSERT INTO users (name, email) VALUES (?, ?)',
    //   ['John Doe', 'john.doe@example.com']
    // );
    // console.log('Users added result', addUsers);

    // const users = await this.platformBridge.database.getQuery<any>(
    //   'SELECT * FROM users',
    //   []
    // );
    // console.log('Database users', JSON.stringify(users));

    // const deletedUSers = await this.platformBridge.database.runQuery(
    //   'DELETE FROM users',
    //   []
    // );
    // console.log('Users deleted result', deletedUSers);

    // === Platform Bridge USB ===
    // console.log('USB ports', await this.platformBridge.usb.getUsbPorts());
    // this.platformBridge.usb.onUsbDeviceConnected(device => {
    //   console.log('USB connected:', device);
    // });

    // this.platformBridge.usb.onUsbDeviceDisconnected(device => {
    //   console.log('USB disconnected:', device);
    // });

    //=== Platform Bridge Serial Port ===
    // console.log(
    //   'Serial Ports',
    //   await this.platformBridge.serialPort.getSerialPorts()
    // );

    // this.platformBridge.serialPort.onSerialPortConnected(portInfo => {
    //   console.log('Serial Port connected', portInfo);
    //   this.platformBridge.serialPort.listenPort(
    //     portInfo.path,
    //     (portPath, err) =>
    //       console.log(`Error on listening Serial Port ${portPath}`, err),
    //     (portPath, data) =>
    //       console.log(`Serial Port ${portPath} data received`, data)
    //   );
    // });

    // this.platformBridge.serialPort.onSerialPortDisconnected(portInfo => {
    //   console.log('Serial Port disconnected', portInfo);
    // });

    console.log(this.platformBridge);

    document.getElementById('connect-usb')?.addEventListener('click', () => {
      (navigator as any).usb
        .requestDevice({ filters: [] })
        .then((devices: any[]) => {
          devices.forEach(device => {
            console.log(device.productName);
            console.log(device.manufacturerName);
          });
        });
    });
  }

  ngOnDestroy(): void {
    this.layoutService.showBreadcrumb();
  }
}
