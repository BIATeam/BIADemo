import { CapacitorSQLite } from '@capacitor-community/sqlite';
import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.bia.biademo',
  appName: 'biademo',
  webDir: 'dist/BIADemo',
  android: {
    allowMixedContent: true,
  },
  server: {
    androidScheme: 'http',
    cleartext: true,
  },
  plugins: {
    CapacitorSQLite: {
      ...CapacitorSQLite,
      androidIsEncryption: false,
      androidBiometric: {
        biometricAuth: false,
      },
    },
  },
};

export default config;