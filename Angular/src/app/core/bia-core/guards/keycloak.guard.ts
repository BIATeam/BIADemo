import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { KeycloakAuthGuard, KeycloakService } from 'keycloak-angular';
import { environment } from 'src/environments/environment';

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
                idpHint: environment.keycloak.login.idpHint
            });
        }

        return this.authenticated;
    }
}