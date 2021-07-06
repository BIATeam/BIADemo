import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GenericDas } from 'src/app/core/bia-core/services/generic-das.service';
import { EnvironmentConfiguration } from '../model/environment-configuration';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentConfigurationDas {
  private route: string;
  constructor(private http: HttpClient) {
    this.route = GenericDas.buildRoute('environment');
  }

  get(): Observable<EnvironmentConfiguration> {
    return this.http.get<EnvironmentConfiguration>(this.route);
  }
}












