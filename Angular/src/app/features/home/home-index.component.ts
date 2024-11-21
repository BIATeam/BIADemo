import { Component, OnDestroy, OnInit } from '@angular/core';
import { BiaOsBridge } from 'src/app/core/bia-core/os-bridges/bia-os-bridge';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  constructor(
    private layoutService: BiaLayoutService,
    private osBridge: BiaOsBridge
  ) {}
  async ngOnInit(): Promise<void> {
    this.layoutService.hideBreadcrumb();

    //  === OS Bridge Database ===

    // const addUsers = await this.osBridge.database.runQuery(
    //   'INSERT INTO users (name, email) VALUES (?, ?)',
    //   ['John Doe', 'john.doe@example.com']
    // );
    // console.log('Users added result', addUsers);

    const users = await this.osBridge.database.getQuery<any>(
      'SELECT * FROM users',
      []
    );
    console.log('Database users', users);

    // === OS Bridge USB ===
    console.log('USB ports', await this.osBridge.usb.getUsbPorts());
    this.osBridge.usb.onUsbDeviceConnected(device => {
      console.log('USB connected:', device);
    });

    this.osBridge.usb.onUsbDeviceDisconnected(device => {
      console.log('USB disconnected:', device);
    });

    // === OS Bridge Serial Port ===
    console.log(
      'Serial Ports',
      await this.osBridge.serialPort.getSerialPorts()
    );

    this.osBridge.serialPort.onSerialPortConnected(portInfo => {
      console.log('Serial Port connected', portInfo);
      this.osBridge.serialPort.listenPort(
        portInfo.path,
        (portPath, err) =>
          console.log(`Error on listening Serial Port ${portPath}`, err),
        (portPath, data) =>
          console.log(`Serial Port ${portPath} data received`, data)
      );
    });

    this.osBridge.serialPort.onSerialPortDisconnected(portInfo => {
      console.log('Serial Port disconnected', portInfo);
    });
  }

  ngOnDestroy(): void {
    this.layoutService.showBreadcrumb();
  }
}
