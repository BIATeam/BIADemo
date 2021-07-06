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

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Plane_List_Access
    },
    component: PlanesIndexComponent,
    canActivate: [PermissionGuard]
  },
  {
    path: 'create',
    data: {
      breadcrumb: 'bia.add',
      canNavigate: false,
      permission: Permission.Plane_Create
    },
    component: PlaneNewComponent,
    canActivate: [PermissionGuard]
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
          permission: Permission.Plane_Update
        },
        component: PlaneEditComponent,
        canActivate: [PermissionGuard]
      },
      {
        path: '',
        redirectTo: 'edit'
      },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    PlaneFormComponent,
    PlanesIndexComponent,
    PlaneItemComponent,
    PlaneNewComponent,
    PlaneEditComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('planes-mode-view', reducers),
    EffectsModule.forFeature([PlanesEffects])
  ]
})
export class PlaneModule { }
