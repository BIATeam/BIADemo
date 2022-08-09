import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { KeycloakAuthGuard, KeycloakService } from 'keycloak-angular';
import { AppSettingsService } from 'src/app/domains/bia-domains/app-settings/services/app-settings.service';

@Injectable({
    providedIn: 'root'
})
export class KeycloakGuard extends KeycloakAuthGuard {

    constructor(
        protected readonly router: Router,
        protected readonly keycloakService: KeycloakService
    ) {
        super(router, keycloakService);
    }

    async isAccessAllowed(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Promise<boolean | UrlTree> {

        if (!this.authenticated) {
            await this.keycloakService.login({
                redirectUri: window.location.origin + state.url,
                idpHint: AppSettingsService.appSettings?.keycloak?.configuration?.idpHint
            });
        }

        return this.authenticated;
    }
}