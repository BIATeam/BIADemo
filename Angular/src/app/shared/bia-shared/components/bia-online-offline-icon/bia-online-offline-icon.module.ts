import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TooltipModule } from 'primeng/tooltip';
import { BiaOnlineOfflineIconComponent } from './bia-online-offline-icon.component';

@NgModule({
  declarations: [BiaOnlineOfflineIconComponent],
  exports: [BiaOnlineOfflineIconComponent],
  imports: [CommonModule, TranslateModule, TooltipModule],
})
export class BiaOnlineOfflineIconModule {}
