import { Injectable, Injector } from '@angular/core';
import { map } from 'rxjs';
import { GenericDas } from './generic-das.service';

@Injectable({
  providedIn: 'root',
})
export class BiaFileDownloaderService extends GenericDas {
  constructor(injector: Injector) {
    super(injector, 'files');
  }

  public downloadFile(guid: string) {
    return this.getItem<string>({ endpoint: `${guid}/getdownloadtoken` }).pipe(
      map(token => {
        const downloadFileUrl = this.route + `${guid}/download?token=${token}`;
        window.open(downloadFileUrl, '_blank');
      })
    );
  }
}
