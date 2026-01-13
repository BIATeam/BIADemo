import { isDevMode } from '@angular/core';
import { provideKeycloak } from 'keycloak-angular';
import { KeycloakConfig } from 'keycloak-js';
import { AppSettingsDas } from '../app-settings/public-api';
import { GenericDas } from './generic-das.service';

export async function loadKeycloakConfig(): Promise<KeycloakConfig | null> {
  try {
    const url = GenericDas.buildRoute(AppSettingsDas.endpoint);
    const response = await fetch(url);

    if (response.ok) {
      const appSettings = await response.json();
      if (appSettings?.keycloak?.isActive) {
        console.info('Keycloak configuration loaded successfully');
        return <KeycloakConfig>{
          url: appSettings.keycloak.baseUrl,
          realm: appSettings.keycloak.configuration.realm,
          clientId: appSettings.keycloak.api.tokenConf.clientId,
        };
      }
    }
  } catch (error) {
    console.warn('Failed to load Keycloak configuration:', error);
  }

  return null;
}

export const provideKeycloakAngular = (config: KeycloakConfig) => {
  return provideKeycloak({
    config: config,
    initOptions: {
      onLoad: 'check-sso',
      checkLoginIframe: false,
      enableLogging: isDevMode(),
    },
  });
};
