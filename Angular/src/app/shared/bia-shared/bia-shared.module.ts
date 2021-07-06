// Modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortalModule } from '@angular/cdk/portal';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpClientModule } from '@angular/common/http';

// PrimeNG Modules
import { AutoCompleteModule } from 'primeng/autocomplete';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { ChipsModule } from 'primeng/chips';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { EditorModule } from 'primeng/editor';
import { FullCalendarModule } from 'primeng/fullcalendar';
import { InputMaskModule } from 'primeng/inputmask';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ListboxModule } from 'primeng/listbox';
import { MegaMenuModule } from 'primeng/megamenu';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { PanelMenuModule } from 'primeng/panelmenu';
import { ProgressBarModule } from 'primeng/progressbar';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SlideMenuModule } from 'primeng/slidemenu';
import { SliderModule } from 'primeng/slider';
import { SpinnerModule } from 'primeng/spinner';
import { SplitButtonModule } from 'primeng/splitbutton';
import { TabMenuModule } from 'primeng/tabmenu';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { ToastModule } from 'primeng/toast';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { FileUploadModule } from 'primeng/fileupload';

// PrimeNG Services
import { MessageService } from 'primeng/api';

// Component
import { BiaTableHeaderComponent } from './components/table/bia-table-header/bia-table-header.component';
import { ClassicFooterComponent } from './components/layout/classic-footer/classic-footer.component';
import { ClassicHeaderComponent } from './components/layout/classic-header/classic-header.component';
import { ClassicLayoutComponent } from './components/layout/classic-layout/classic-layout.component';
import { ClassicPageLayoutComponent } from './components/layout/classic-page-layout/classic-page-layout.component';
import { IeWarningComponent } from './components/layout/classic-header/ie-warning/ie-warning.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { BiaTableControllerComponent } from './components/table/bia-table-controller/bia-table-controller.component';
import { BiaTableComponent } from './components/table/bia-table/bia-table.component';
import { LayoutComponent } from './components/layout/layout.component';
import { PageLayoutComponent } from './components/layout/page-layout.component';
import { PrimengCalendarLocaleDirective } from './directives/primeng-calendar-locale.directive';
import { ViewListComponent } from './features/view/views/view-list/view-list.component';
import { ViewDialogComponent } from './features/view/views/view-dialog/view-dialog.component';
import { ViewSiteTableComponent } from './features/view/components/view-site-table/view-site-table.component';
import { ViewUserTableComponent } from './features/view/components/view-user-table/view-user-table.component';
import { StoreModule } from '@ngrx/store';
import { reducers } from './features/view/store/view.state';
import { EffectsModule } from '@ngrx/effects';
import { ViewsEffects } from './features/view/store/views-effects';
import { ViewFormComponent } from './features/view/components/view-form/view-form.component';
import { BiaCalcTableComponent } from './components/table/bia-calc-table/bia-calc-table.component';
import { TranslateRoleLabelPipe } from './pipes/translate-role-label.pipe';
import { PopupLayoutComponent } from './components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from './components/layout/fullpage-layout/fullpage-layout.component';
import { PluckPipe } from './pipes/pluck.pipe';
import { JoinPipe } from './pipes/join.pipe';

const PRIMENG_MODULES = [
  AutoCompleteModule,
  BreadcrumbModule,
  ButtonModule,
  CalendarModule,
  CheckboxModule,
  ChipsModule,
  CodeHighlighterModule,
  ConfirmDialogModule,
  ContextMenuModule,
  DialogModule,
  DropdownModule,
  EditorModule,
  FullCalendarModule,
  InputMaskModule,
  InputSwitchModule,
  InputTextModule,
  InputTextareaModule,
  ListboxModule,
  MegaMenuModule,
  MenuModule,
  MenubarModule,
  MessageModule,
  MessagesModule,
  MultiSelectModule,
  PaginatorModule,
  PanelModule,
  PanelMenuModule,
  ProgressBarModule,
  RadioButtonModule,
  ScrollPanelModule,
  SelectButtonModule,
  SlideMenuModule,
  SliderModule,
  SpinnerModule,
  SplitButtonModule,
  TableModule,
  TabMenuModule,
  TabViewModule,
  TieredMenuModule,
  ToastModule,
  ToggleButtonModule,
  ToolbarModule,
  TooltipModule,
  FileUploadModule
];

const MODULES = [
  CommonModule,
  PortalModule,
  TranslateModule,
  FormsModule,
  ReactiveFormsModule,
  FlexLayoutModule,
  HttpClientModule
];

const COMPONENTS = [
  ClassicFooterComponent,
  ClassicHeaderComponent,
  ClassicLayoutComponent,
  ClassicPageLayoutComponent,
  SpinnerComponent,
  IeWarningComponent,
  BiaTableComponent,
  BiaCalcTableComponent,
  BiaTableHeaderComponent,
  BiaTableControllerComponent,
  LayoutComponent,
  PageLayoutComponent,
  PopupLayoutComponent,
  FullPageLayoutComponent,
  PrimengCalendarLocaleDirective
];

const VIEW_COMPONENTS = [
  ViewListComponent,
  ViewDialogComponent,
  ViewSiteTableComponent,
  ViewUserTableComponent,
  ViewFormComponent
];

const PIPES = [
  TranslateRoleLabelPipe,
  PluckPipe,
  JoinPipe
];

const VIEW_IMPORTS = [StoreModule.forFeature('views', reducers), EffectsModule.forFeature([ViewsEffects])];

const SERVICES = [MessageService];

@NgModule({
  imports: [...PRIMENG_MODULES, ...MODULES, ...VIEW_IMPORTS],
  declarations: [...COMPONENTS, ...VIEW_COMPONENTS, ...PIPES],
  exports: [...PRIMENG_MODULES, ...MODULES, ...COMPONENTS, ...PIPES],
  providers: [...SERVICES]
})

// https://medium.com/@benmohamehdi/angular-best-practices-coremodule-vs-sharedmodule-25f6721aa2ef
export class BiaSharedModule {}
