import { CommonModule } from '@angular/common';
import {
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { BadgeModule } from 'primeng/badge';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DrawerModule } from 'primeng/drawer';
import { InputTextModule } from 'primeng/inputtext';
import { MegaMenuModule } from 'primeng/megamenu';
import { MenuModule } from 'primeng/menu';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RippleModule } from 'primeng/ripple';
import { StyleClassModule } from 'primeng/styleclass';
import { ToastModule } from 'primeng/toast';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { TooltipModule } from 'primeng/tooltip';
import { BiaOnlineOfflineIconModule } from '../../bia-online-offline-icon/bia-online-offline-icon.module';
import { BiaTeamSelectorModule } from '../../bia-team-selector/bia-team-selector.module';
import { NotificationTeamWarningModule } from '../../notification-team-warning/notification-team-warning.module';
import { IeWarningComponent } from '../ie-warning/ie-warning.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    StyleClassModule,
    InputTextModule,
    BadgeModule,
    DrawerModule,
    RadioButtonModule,
    ToggleSwitchModule,
    TooltipModule,
    MegaMenuModule,
    RippleModule,
    RouterModule,
    ButtonModule,
    MenuModule,
    BreadcrumbModule,
    IeWarningComponent,
    BiaOnlineOfflineIconModule,
    BiaTeamSelectorModule,
    ToastModule,
    NotificationTeamWarningModule,
    DialogModule,
  ],
  providers: [provideHttpClient(withInterceptorsFromDi())],
})
export class BiaUltimaLayoutModule {}
