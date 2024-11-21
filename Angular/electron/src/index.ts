import type { CapacitorElectronConfig } from '@capacitor-community/electron';
import {
  getCapacitorElectronConfig,
  setupElectronDeepLinking,
} from '@capacitor-community/electron';
import type { MenuItemConstructorOptions } from 'electron';
import { app, ipcMain, MenuItem } from 'electron';
import electronIsDev from 'electron-is-dev';
import unhandled from 'electron-unhandled';
import { autoUpdater } from 'electron-updater';
import { SerialService } from './serial.service';
import { ElectronCapacitorApp, setupReloadWatcher } from './setup';
import { SqliteDal } from './sqlite.dal';
import { UsbService } from './usb.service';

process.env['ELECTRON_DISABLE_SECURITY_WARNINGS'] = 'true';

// Graceful handling of unhandled errors.
unhandled();

// Define our menu templates (these are optional)
const trayMenuTemplate: (MenuItemConstructorOptions | MenuItem)[] = [
  new MenuItem({ label: 'Quit App', role: 'quit' }),
];
const appMenuBarMenuTemplate: (MenuItemConstructorOptions | MenuItem)[] = [
  { role: process.platform === 'darwin' ? 'appMenu' : 'fileMenu' },
  { role: 'viewMenu' },
];

// Get Config options from capacitor.config
const capacitorFileConfig: CapacitorElectronConfig =
  getCapacitorElectronConfig();

// Initialize our app. You can pass menu templates into the app here.
// const myCapacitorApp = new ElectronCapacitorApp(capacitorFileConfig);
const electronCapacitorApp = new ElectronCapacitorApp(
  capacitorFileConfig,
  trayMenuTemplate,
  appMenuBarMenuTemplate
);

// If deeplinking is enabled then we will set it up here.
if (capacitorFileConfig.electron?.deepLinkingEnabled) {
  setupElectronDeepLinking(electronCapacitorApp, {
    customProtocol:
      capacitorFileConfig.electron.deepLinkingCustomProtocol ??
      'mycapacitorapp',
  });
}

// If we are in Dev mode, use the file watcher components.
if (electronIsDev) {
  setupReloadWatcher(electronCapacitorApp);
}

// Run Application
(async () => {
  // Wait for electron app to be ready.
  await app.whenReady();
  // Security - Set Content-Security-Policy based on whether or not we are in dev mode.
  // TODO uncomment following when CORS errors resolved
  //setupContentSecurityPolicy(myCapacitorApp.getCustomURLScheme());
  // Initialize our app, build windows, and load content.
  await electronCapacitorApp.init();
  // Check for updates if we are in a packaged app.
  autoUpdater.checkForUpdatesAndNotify();
})();

// Handle when all of our windows are close (platforms have their own expectations).
app.on('window-all-closed', function () {
  // On OS X it is common for applications and their menu bar
  // to stay active until the user quits explicitly with Cmd + Q
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

// When the dock icon is clicked.
app.on('activate', async function () {
  // On OS X it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  if (electronCapacitorApp.getMainWindow().isDestroyed()) {
    await electronCapacitorApp.init();
  }
});

// Place all ipc or other electron api calls and custom functionality under this line
useUsbService(electronCapacitorApp);
useDatabase();
useSerialService(electronCapacitorApp);

function useUsbService(electronCapacitorApp: ElectronCapacitorApp) {
  const usbService = new UsbService(electronCapacitorApp);
  usbService.init();

  ipcMain.handle('get-usb-ports', usbService.getUsbPorts);
}

function useDatabase() {
  const sqliteDal = new SqliteDal(
    'C:\\temp\\CapacitorDatabases\\biademo\\myuserdbSQLite.db'
  );
  sqliteDal.initialize();

  ipcMain.handle('db:run', async (_event, query, params) => {
    try {
      const result = await sqliteDal.runQuery(query, params);
      return result;
    } catch (error) {
      console.error('Error running query:', error);
      throw error;
    }
  });

  ipcMain.handle('db:get', async (_event, query, params) => {
    try {
      const result = await sqliteDal.getQuery(query, params);
      return result;
    } catch (error) {
      console.error('Error fetching data:', error);
      throw error;
    }
  });
}

function useSerialService(electronCapacitorApp: ElectronCapacitorApp) {
  const serialService = new SerialService(electronCapacitorApp);
  serialService.init();

  ipcMain.handle('get-serial-ports', serialService.getPorts);
  ipcMain.handle('listen-port', (_event, portPath) =>
    serialService.listen(portPath)
  );
}
