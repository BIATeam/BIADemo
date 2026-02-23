import { Injectable, Injector } from '@angular/core';
import { GenericDas } from 'packages/bia-ng/core/public-api';

@Injectable({
  providedIn: 'root',
})
export class ExamplesDas extends GenericDas {
  constructor(injector: Injector) {
    super(injector, 'examples');
  }

  generateUnhandledError() {
    return this.http.get(this.route + 'GenerateUnhandledError');
  }

  generateHandledError() {
    return this.http.get(this.route + 'GenerateHandledError');
  }

  generateFileDownloadNotification() {
    return this.http.post(
      this.route + 'GenerateFileDownloadNotification',
      null
    );
  }
}
