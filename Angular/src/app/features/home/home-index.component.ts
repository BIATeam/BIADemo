import { Component, OnDestroy, OnInit } from '@angular/core';
import { BiaPlatformBridge } from 'src/app/core/bia-core/platform-bridges/bia.platform-bridge';
import { BiaDirectorySystemService } from 'src/app/core/bia-core/services/bia-directory-system.service';
import { BiaFileSystemService } from 'src/app/core/bia-core/services/bia-file-system.service';
import { biaDatabase } from 'src/app/core/bia-core/services/bia.database';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  constructor(
    private layoutService: BiaLayoutService,
    private platformBridge: BiaPlatformBridge,
    private fileSystemService: BiaFileSystemService,
    private directorySystemService: BiaDirectorySystemService
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

    //=== Platform Bridge USB ===
    // console.log('USB ports', await this.platformBridge.usb.getUsbPorts());

    // this.platformBridge.usb.onUsbDeviceConnected(device => {
    //   console.log('USB connected:', device);
    // });

    // this.platformBridge.usb.onUsbDeviceDisconnected(device => {
    //   console.log('USB disconnected:', device);
    // });

    //=== Platform Bridge Serial Port ===
    console.log(
      'Serial Ports',
      await this.platformBridge.serialPort.getSerialPorts()
    );

    this.platformBridge.serialPort.onSerialPortConnected(portInfo => {
      console.log('Serial Port connected', portInfo);
      this.platformBridge.serialPort.listenPort(
        portInfo,
        (portPath, err) =>
          console.log(`Error on listening Serial Port ${portPath}`, err),
        (portPath, data) => this.eInsertPortRead(data)
      );
    });

    this.platformBridge.serialPort.onSerialPortDisconnected(portInfo => {
      console.log('Serial Port disconnected', portInfo);
    });

    document.getElementById('connect-usb')?.addEventListener('click', () => {
      (navigator as any).usb
        .requestDevice({ filters: [] })
        .then((devices: any[]) => {
          console.log('Usb devices requested : ', devices);
        });
    });

    document.getElementById('connect-serial')?.addEventListener('click', () => {
      (navigator as any).serial
        .requestPort({ filters: [] })
        .then((devices: any[]) => {
          console.log('Serial ports requested', devices);
        });
    });

    const folderHandler = await biaDatabase.getDirectory('user');
    if (folderHandler) {
      this.directorySystemService.setHandler(folderHandler);
    }
  }

  ngOnDestroy(): void {
    this.layoutService.showBreadcrumb();
  }

  fileContent: string | null = null;
  async openFile() {
    const file = await this.fileSystemService.openFilePicker();
    if (file) {
      console.log('File selected:', file.name);
    }
  }

  async readFile() {
    this.fileContent = await this.fileSystemService.readFile();
  }

  async writeFile() {
    const content = 'New file content!';
    await this.fileSystemService.writeFile(content);
  }

  folderContents: string[] = [];
  async openFolder() {
    const directoryHandle =
      await this.directorySystemService.openDirectoryPicker();
    if (directoryHandle) {
      console.log('Folder selected:', directoryHandle.name);
      biaDatabase.saveDirectory('user', directoryHandle, {
        timestamp: Date.now(),
      });
    }
  }

  async addFile() {
    await this.directorySystemService.addFile(
      'newFile.txt',
      'This is some content for the file.'
    );
  }

  async deleteFile() {
    await this.directorySystemService.deleteEntry('newFile.txt');
  }

  async listFiles() {
    this.folderContents =
      await this.directorySystemService.readDirectoryContents();
  }

  private eInsertPortRead(data: any) {
    if (data instanceof Uint8Array) {
      const dataArray = data as Uint8Array;
      if (dataArray.buffer.byteLength === 10) {
        console.log('data', dataArray);
      }
    }
  }
}
