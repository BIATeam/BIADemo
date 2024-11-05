import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { BadgeModule } from 'primeng/badge';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { MegaMenuModule } from 'primeng/megamenu';
import { MenuModule } from 'primeng/menu';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RippleModule } from 'primeng/ripple';
import { SidebarModule } from 'primeng/sidebar';
import { StyleClassModule } from 'primeng/styleclass';
import { ToastModule } from 'primeng/toast';
import { TooltipModule } from 'primeng/tooltip';
import { BiaOnlineOfflineIconModule } from '../../bia-online-offline-icon/bia-online-offline-icon.module';
import { BiaTeamSelectorModule } from '../../bia-team-selector/bia-team-selector.module';
import { NotificationTeamWarningModule } from '../../notification-team-warning/notification-team-warning.module';
import { IeWarningComponent } from '../ie-warning/ie-warning.component';
import { BiaUltimaConfigComponent } from './config/ultima-config.component';
import { BiaUltimaFooterComponent } from './footer/ultima-footer.component';
import { BiaUltimaLayoutComponent } from './layout/ultima-layout.component';
import { BiaUltimaMenuItemComponent } from './menu-item/ultima-menu-item.component';
import { BiaUltimaMenuProfileComponent } from './menu-profile/ultima-menu-profile.component';
import { BiaUltimaMenuComponent } from './menu/ultima-menu.component';
import { BiaUltimaSideBarComponent } from './side-bar/ultima-side-bar.component';
import { BiaUltimaTopBarComponent } from './top-bar/ultima-top-bar.component';

@NgModule({
  declarations: [
    BiaUltimaLayoutComponent,
    BiaUltimaTopBarComponent,
    BiaUltimaMenuComponent,
    BiaUltimaSideBarComponent,
    BiaUltimaMenuItemComponent,
    BiaUltimaFooterComponent,
    BiaUltimaMenuProfileComponent,
    BiaUltimaConfigComponent,
  ],
  exports: [
    BiaUltimaLayoutComponent,
    BiaUltimaTopBarComponent,
    BiaUltimaMenuComponent,
    BiaUltimaSideBarComponent,
    BiaUltimaMenuItemComponent,
    BiaUltimaFooterComponent,
    BiaUltimaMenuProfileComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    TranslateModule,
    StyleClassModule,
    InputTextModule,
    SidebarModule,
    BadgeModule,
    RadioButtonModule,
    InputSwitchModule,
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
})
export class BiaUltimaLayoutModule {}
