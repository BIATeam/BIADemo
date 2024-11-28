// Modules
import { PortalModule } from '@angular/cdk/portal';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

// PrimeNG Modules
import { AccordionModule } from 'primeng/accordion';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { BadgeModule } from 'primeng/badge';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
// import { ChipsModule } from 'primeng/chips';
// import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
// import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
// import { EditorModule } from 'primeng/editor';
// Warning it requiered to install Quill package:
//    - npm install quill
// And modify angular.json to add quill css and js :
// "styles": [
//   "node_modules/primeng/resources/primeng.min.css",
//   "node_modules/primeicons/primeicons.css",
//+   "node_modules/quill/dist/quill.core.css",
//+   "node_modules/quill/dist/quill.snow.css",
//   "src/styles.scss"
// ],
// "scripts": [
//+   "node_modules/quill/dist/quill.js"
// ],

import { FieldsetModule } from 'primeng/fieldset';
// import { FullCalendarModule } from 'primeng/fullcalendar';
// Warning it requiered to install Fullcalandar package:
//    - npm install @fullcalendar/core
//    - npm install @fullcalendar/daygrid
//    - npm install @fullcalendar/interaction
//    - npm install @fullcalendar/timegrid
// import { InputMaskModule } from 'primeng/inputmask';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ListboxModule } from 'primeng/listbox';
import { MegaMenuModule } from 'primeng/megamenu';
// import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
// import { MessagesModule } from 'primeng/messages';
// import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
// import { PaginatorModule } from 'primeng/paginator';
// import { PanelModule } from 'primeng/panel';
// import { PanelMenuModule } from 'primeng/panelmenu';
// import { ProgressBarModule } from 'primeng/progressbar';
import { RadioButtonModule } from 'primeng/radiobutton';
// import { ScrollPanelModule } from 'primeng/scrollpanel';
// import { SelectButtonModule } from 'primeng/selectbutton';
// import { SlideMenuModule } from 'primeng/slidemenu';
// import { SliderModule } from 'primeng/slider';
// import { SpinnerModule } from 'primeng/spinner';
// import { SplitButtonModule } from 'primeng/splitbutton';
// import { TabMenuModule } from 'primeng/tabmenu';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
// import { TieredMenuModule } from 'primeng/tieredmenu';
import { ToastModule } from 'primeng/toast';
import { ToggleButtonModule } from 'primeng/togglebutton';
// import { ToolbarModule } from 'primeng/toolbar';
// import { TooltipModule } from 'primeng/tooltip';
import { FileUploadModule } from 'primeng/fileupload';

// PrimeNG Services
import { MessageService } from 'primeng/api';

// Component
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SkeletonModule } from 'primeng/skeleton';
import { TriStateCheckboxModule } from 'primeng/tristatecheckbox';
import { NotificationsEffects } from 'src/app/domains/bia-domains/notification/store/notifications-effects';
import { TeamModule } from 'src/app/domains/bia-domains/team/team.module';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';
import { reducers as notificationReducers } from '../../domains/bia-domains/notification/store/notification.state';
import { BiaOnlineOfflineIconModule } from './components/bia-online-offline-icon/bia-online-offline-icon.module';
import { BiaTeamSelectorModule } from './components/bia-team-selector/bia-team-selector.module';
import { BiaFieldBaseComponent } from './components/form/bia-field-base/bia-field-base.component';
import { BiaFormComponent } from './components/form/bia-form/bia-form.component';
import { BiaInputComponent } from './components/form/bia-input/bia-input.component';
import { BiaOutputComponent } from './components/form/bia-output/bia-output.component';
import { HangfireContainerComponent } from './components/hangfire-container/hangfire-container.component';
import { ClassicFooterComponent } from './components/layout/classic-footer/classic-footer.component';
import { ClassicHeaderComponent } from './components/layout/classic-header/classic-header.component';
import { ClassicLayoutComponent } from './components/layout/classic-layout/classic-layout.component';
import { ClassicPageLayoutComponent } from './components/layout/classic-page-layout/classic-page-layout.component';
import { FullPageLayoutComponent } from './components/layout/fullpage-layout/fullpage-layout.component';
import { IeWarningComponent } from './components/layout/ie-warning/ie-warning.component';
import { LayoutComponent } from './components/layout/layout.component';
import { PageLayoutComponent } from './components/layout/page-layout.component';
import { PopupLayoutComponent } from './components/layout/popup-layout/popup-layout.component';
import { BiaScrollingNotificationComponent } from './components/layout/scrolling-notification/scrolling-notification.component';
import { BiaUltimaLayoutModule } from './components/layout/ultima/ultima-layout.module';
import { NotificationTeamWarningModule } from './components/notification-team-warning/notification-team-warning.module';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { BiaCalcTableComponent } from './components/table/bia-calc-table/bia-calc-table.component';
import { BiaTableBehaviorControllerComponent } from './components/table/bia-table-behavior-controller/bia-table-behavior-controller.component';
import { BiaTableControllerComponent } from './components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableFilterComponent } from './components/table/bia-table-filter/bia-table-filter.component';
import { BiaTableFooterControllerComponent } from './components/table/bia-table-footer-controller/bia-table-footer-controller.component';
import { BiaTableHeaderComponent } from './components/table/bia-table-header/bia-table-header.component';
import { BiaTableInputComponent } from './components/table/bia-table-input/bia-table-input.component';
import { BiaTableOutputComponent } from './components/table/bia-table-output/bia-table-output.component';
import { BiaTableComponent } from './components/table/bia-table/bia-table.component';
import { TeamAdvancedFilterComponent } from './components/team-advanced-filter/team-advanced-filter.component';
import { ViewFormComponent } from './features/view/components/view-form/view-form.component';
import { ViewTeamTableComponent } from './features/view/components/view-team-table/view-team-table.component';
import { ViewUserTableComponent } from './features/view/components/view-user-table/view-user-table.component';
import { reducers } from './features/view/store/view.state';
import { ViewsEffects } from './features/view/store/views-effects';
import { ViewDialogComponent } from './features/view/views/view-dialog/view-dialog.component';
import { ViewListComponent } from './features/view/views/view-list/view-list.component';
import { FormatValuePipe } from './pipes/format-value.pipe';
import { JoinPipe } from './pipes/join.pipe';
import { PluckPipe } from './pipes/pluck.pipe';
import { SafeUrlPipe } from './pipes/safe-url.pipe';
import { TranslateFieldPipe } from './pipes/translate-field.pipe';

const PRIMENG_MODULES = [
  AccordionModule,
  AutoCompleteModule,
  BadgeModule,
  BreadcrumbModule,
  ButtonModule,
  CalendarModule,
  CheckboxModule,
  //  ChipsModule,
  //  CodeHighlighterModule,
  ConfirmDialogModule,
  //  ContextMenuModule,
  DialogModule,
  DropdownModule,
  //  EditorModule,
  FieldsetModule,
  //  FullCalendarModule,
  //  InputMaskModule,
  InputSwitchModule,
  InputTextModule,
  InputTextareaModule,
  InputNumberModule,
  ListboxModule,
  MegaMenuModule,
  //  MenuModule,
  MenubarModule,
  //  MessageModule,
  //  MessagesModule,
  MultiSelectModule,
  //  PaginatorModule,
  //  PanelModule,
  //  PanelMenuModule,
  //  ProgressBarModule,
  RadioButtonModule,
  //  ScrollPanelModule,
  //  SelectButtonModule,
  //  SlideMenuModule,
  //  SliderModule,
  //  SpinnerModule,
  //  SplitButtonModule,
  TableModule,
  //  TabMenuModule,
  TabViewModule,
  //  TieredMenuModule,
  ToastModule,
  ToggleButtonModule,
  //  ToolbarModule,
  //  TooltipModule,
  FileUploadModule,
  SkeletonModule,
  TriStateCheckboxModule,
];

const MODULES = [
  CommonModule,
  PortalModule,
  TranslateModule,
  FormsModule,
  ReactiveFormsModule,
  HttpClientModule,
  TeamModule,
  //BiaLayoutModule,
  BiaTeamSelectorModule,
  BiaOnlineOfflineIconModule,
  NotificationTeamWarningModule,
  BiaUltimaLayoutModule,
];

const COMPONENTS = [
  ClassicFooterComponent,
  ClassicHeaderComponent,
  ClassicLayoutComponent,
  ClassicPageLayoutComponent,
  SpinnerComponent,
  BiaTableComponent,
  BiaTableFilterComponent,
  BiaFieldBaseComponent,
  BiaFormComponent,
  BiaInputComponent,
  BiaOutputComponent,
  BiaTableInputComponent,
  BiaTableOutputComponent,
  BiaCalcTableComponent,
  BiaTableHeaderComponent,
  BiaTableControllerComponent,
  BiaTableFooterControllerComponent,
  BiaTableBehaviorControllerComponent,
  LayoutComponent,
  PageLayoutComponent,
  PopupLayoutComponent,
  FullPageLayoutComponent,
  HangfireContainerComponent,
  TeamAdvancedFilterComponent,
  BiaScrollingNotificationComponent,
];

const VIEW_COMPONENTS = [
  ViewListComponent,
  ViewDialogComponent,
  ViewTeamTableComponent,
  ViewUserTableComponent,
  ViewFormComponent,
];

const PIPES = [
  PluckPipe,
  JoinPipe,
  TranslateFieldPipe,
  FormatValuePipe,
  SafeUrlPipe,
];

const VIEW_IMPORTS = [
  StoreModule.forFeature('views', reducers),
  EffectsModule.forFeature([ViewsEffects]),
];

const NOTIFICATION_IMPORTS = [
  StoreModule.forFeature('domain-notifications', notificationReducers),
  EffectsModule.forFeature([NotificationsEffects]),
];

const TEAM_ADVANCED_FILTER_IMPORTS = [UserOptionModule];
const SERVICES = [MessageService];

const STANDALONE_COMPONENTS = [IeWarningComponent];

@NgModule({
  imports: [
    ...PRIMENG_MODULES,
    ...MODULES,
    ...VIEW_IMPORTS,
    ...NOTIFICATION_IMPORTS,
    ...TEAM_ADVANCED_FILTER_IMPORTS,
    ...STANDALONE_COMPONENTS,
  ],
  declarations: [...COMPONENTS, ...VIEW_COMPONENTS, ...PIPES],
  exports: [
    ...PRIMENG_MODULES,
    ...MODULES,
    ...COMPONENTS,
    ...VIEW_COMPONENTS,
    ...PIPES,
  ],
  providers: [...SERVICES],
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class BiaSharedModule {}
