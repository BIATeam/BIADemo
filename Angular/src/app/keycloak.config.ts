import { KeycloakConfig } from 'keycloak-js';
import { AppSettingsDas, GenericDas } from 'packages/bia-ng/core/public-api';

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
