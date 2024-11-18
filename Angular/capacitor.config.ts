import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.bia.biademo',
  appName: 'biademo',
  webDir: 'dist/BIADemo',
  plugins: {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    CapacitorSQLite: {
      electronIsEncryption: false,
      electronWindowsLocation: 'C:\\temp\\CapacitorDatabases',
    },
  },
};

export default config;
