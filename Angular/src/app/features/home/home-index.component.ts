import { Component, OnDestroy, OnInit } from '@angular/core';
import { BiaOsBridge } from 'src/app/core/bia-core/os-bridges/bia-os-bridge';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  ports: any[] = [];

  constructor(
    private layoutService: BiaLayoutService,
    private osBridge: BiaOsBridge
  ) {}
  ngOnInit(): void {
    this.layoutService.hideBreadcrumb();
    this.osBridge.usb.getUsbPorts().then(x => {
      this.ports = x.map(y => JSON.stringify(y));
    });

    this.osBridge.usb.onUsbDeviceConnected(device => {
      console.log('Device Connected:', device);
    });

    this.osBridge.usb.onUsbDeviceDisconnected(device => {
      console.log('Device Disconnected:', device);
    });
  }

  ngOnDestroy(): void {
    this.layoutService.showBreadcrumb();
  }
}
