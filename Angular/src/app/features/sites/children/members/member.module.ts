import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { MembersEffects } from './store/members-effects';
import { reducers } from './store/member.state';
import { MemberFormComponent } from './components/member-form/member-form.component';
import { MembersIndexComponent } from './views/members-index/members-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MemberNewComponent } from './views/member-new/member-new.component';
import { MemberEditComponent } from './views/member-edit/member-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { MemberItemComponent } from './views/member-item/member-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { MemberTableComponent } from './components/member-table/member-table.component';
import { RoleOptionModule } from 'src/app/domains/role-option/role-option.module';
import { UserOptionModule } from 'src/app/domains/user-option/user-option.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Member_List_Access,
      InjectComponent: MembersIndexComponent
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
          permission: Permission.Member_Create,
          title: 'member.add',
          InjectComponent: MemberNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':memberId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: MemberItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'member.manage',
              canNavigate: true,
              permission: Permission.Member_Update,
              title: 'member.manage',
              InjectComponent: MemberEditComponent,
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
    MemberItemComponent,
    // [Calc] : NOT used only for calc (4 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    MemberFormComponent,
    MembersIndexComponent,
    MemberNewComponent,
    MemberEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    MemberTableComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('members', reducers),
    EffectsModule.forFeature([MembersEffects]),
    // Domain Modules:
    UserOptionModule,
    RoleOptionModule,
  ]
})
export class MemberModule {
}

