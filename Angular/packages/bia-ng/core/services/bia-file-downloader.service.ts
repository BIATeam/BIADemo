import { DestroyRef, Injectable, Injector, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { catchError, finalize, first, map } from 'rxjs';
import { BiaMessageService } from './bia-message.service';
import { GenericDas } from './generic-das.service';

@Injectable({
  providedIn: 'root',
})
export class BiaFileDownloaderService extends GenericDas {
  private destroyRef = inject(DestroyRef);
  private biaMessageService = inject(BiaMessageService);

  constructor(injector: Injector) {
    super(injector, 'files');
  }

  public downloadFile(guid: string, onComplete?: () => void): void {
    this.getItem<string>({ endpoint: `${guid}/getdownloadtoken` })
      .pipe(
        first(),
        map(token => {
          const downloadFileUrl =
            this.route + `${guid}/download?token=${token}`;
          window.open(downloadFileUrl, '_blank');
        }),
        catchError(err => {
          this.biaMessageService.showErrorHttpResponse(err);
          return [];
        }),
        finalize(() => {
          onComplete?.();
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe();
  }
}
