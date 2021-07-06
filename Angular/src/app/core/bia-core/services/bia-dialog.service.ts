import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Confirmation } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class BiaDialogService {
  constructor(private translateService: TranslateService) {}

  public getDeleteConfirmation(key?: string): Confirmation {
    const translates = this.translateService.instant(['biaMsg.confirmDelete', 'bia.confirm', 'bia.yes', 'bia.no']);
    const confirmation: Confirmation = {
      key: key,
      message: translates['biaMsg.confirmDelete'],
      header: translates['bia.confirm'],
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: translates['bia.yes'],
      rejectLabel: translates['bia.no']
    };
    return confirmation;
  }
}
