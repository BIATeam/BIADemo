import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { MembersEffects } from './store/members-effects';
import { reducers } from './store/member.state';
import { MemberEditDialogComponent } from './views/member-edit-dialog/member-edit-dialog.component';
import { MemberNewDialogComponent } from './views/member-new-dialog/member-new-dialog.component';
import { MembersIndexComponent } from './views/members-index/members-index.component';
import { MemberEditFormComponent } from './components/member-edit-form/member-edit-form.component';
import { MemberNewFormComponent } from './components/member-new-form/member-new-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserModule } from 'src/app/domains/user/user.module';
import { RoleModule } from 'src/app/domains/role/role.module';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { Permission } from 'src/app/shared/permission';
import { UserFromADModule } from 'src/app/domains/user-from-AD/user-from-AD.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Member_List_Access
    },
    component: MembersIndexComponent,
    canActivate: [PermissionGuard]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    MemberEditDialogComponent,
    MemberNewDialogComponent,
    MembersIndexComponent,
    MemberEditFormComponent,
    MemberNewFormComponent,
  ],
  imports: [
    SharedModule,
    RoleModule,
    UserModule,
    UserFromADModule,
    RouterModule.forChild(ROUTES),
    StoreModule.forFeature('members', reducers),
    EffectsModule.forFeature([MembersEffects])
  ]
})
export class MemberModule { }
