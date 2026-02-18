import { DestroyRef, Injectable, Injector, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { first, map } from 'rxjs';
import { GenericDas } from './generic-das.service';

@Injectable({
  providedIn: 'root',
})
export class BiaFileDownloaderService extends GenericDas {
  private destroyRef = inject(DestroyRef);

  constructor(injector: Injector) {
    super(injector, 'files');
  }

  public downloadFile(guid: string): void {
    this.getItem<string>({ endpoint: `${guid}/getdownloadtoken` })
      .pipe(
        first(),
        map(token => {
          const downloadFileUrl =
            this.route + `${guid}/download?token=${token}`;
          window.open(downloadFileUrl, '_blank');
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}
