import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { PlanesEffects } from './store/planes-effects';
import { reducers } from './store/plane.state';
import { PlaneFormComponent } from './components/plane-form/plane-form.component';
import { PlanesIndexComponent } from './views/planes-index/planes-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PlaneNewComponent } from './views/plane-new/plane-new.component';
import { PlaneEditComponent } from './views/plane-edit/plane-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { PlaneItemComponent } from './views/plane-item/plane-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { AirportOptionModule } from 'src/app/domains/airport-option/airport-option.module';
import { PlaneTypeOptionModule } from 'src/app/domains/plane-type-option/plane-type-option.module';
import { PlaneTableComponent } from './components/plane-table/plane-table.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Plane_List_Access,
      InjectComponent: PlanesIndexComponent
    },
    component: FullPageLayoutComponent,
    canActivate: [PermissionGuard],
    // [Calc] : The children are not used in calc
    children: [
      {
        path: 'create',
        data: {
          breadcrumb: 'bia.add',
          canNavigate: false,
          permission: Permission.Plane_Create,
          title: 'plane.add',
          InjectComponent: PlaneNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':planeId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: PlaneItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'bia.edit',
              canNavigate: true,
              permission: Permission.Plane_Update,
              title: 'plane.edit',
              InjectComponent: PlaneEditComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            redirectTo: 'edit'
          },
        ]
      },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    PlaneItemComponent,
    // [Calc] : NOT used only for calc (4 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    PlaneFormComponent,
    PlanesIndexComponent,
    PlaneNewComponent,
    PlaneEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    PlaneTableComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('planes', reducers),
    EffectsModule.forFeature([PlanesEffects]),
    // Domain Modules:
    AirportOptionModule,
    PlaneTypeOptionModule,
  ]
})
export class PlaneModule {
}

