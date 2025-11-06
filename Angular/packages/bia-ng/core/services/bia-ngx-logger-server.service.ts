import { HttpBackend, HttpRequest } from '@angular/common/http';
import { inject, Injectable, NgZone } from '@angular/core';
import Keycloak from 'keycloak-js';
import { NGXLoggerServerService } from 'ngx-logger';
import { Observable } from 'rxjs';
import { AppSettingsService } from '../app-settings/services/app-settings.service';

@Injectable({
  providedIn: 'root',
})
export class BiaNgxLoggerServerService extends NGXLoggerServerService {
  private readonly keycloakService = inject(Keycloak);
  constructor(
    protected httpBackend: HttpBackend,
    protected appSettingsService: AppSettingsService,
    protected ngZone: NgZone
  ) {
    super(httpBackend, ngZone);
  }

  protected override alterHttpRequest(
    httpRequest: HttpRequest<any>
  ): HttpRequest<any> | Observable<HttpRequest<any>> {
    if (
      this.appSettingsService.appSettings?.keycloak?.isActive === true &&
      this.keycloakService.token
    ) {
      return this.addToken(httpRequest, this.keycloakService.token);
    } else {
      return super.alterHttpRequest(httpRequest);
    }
  }

  protected addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      withCredentials: false,
      setHeaders: {
        // eslint-disable-next-line @typescript-eslint/naming-convention
        Authorization: `Bearer ${token}`,
      },
    });
  }
}
