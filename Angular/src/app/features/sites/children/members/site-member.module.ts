import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SiteMembersIndexComponent } from './views/site-members-index/site-members-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { SiteMemberNewComponent } from './views/site-member-new/site-member-new.component';
import { SiteMemberEditComponent } from './views/site-member-edit/site-member-edit.component';
import { Permission } from 'src/app/shared/permission';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { SiteMemberItemComponent } from './views/site-member-item/site-member-item.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { MemberModule } from 'src/app/shared/bia-shared/feature-templates/members/member.module';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Site_Member_List_Access,
      InjectComponent: SiteMembersIndexComponent
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
          permission: Permission.Site_Member_Create,
          title: 'member.add',
          InjectComponent: SiteMemberNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: ':crudItemId',
        data: {
          breadcrumb: '',
          canNavigate: true,
        },
        component: SiteMemberItemComponent,
        canActivate: [PermissionGuard],
        children: [
          {
            path: 'edit',
            data: {
              breadcrumb: 'member.manage',
              canNavigate: true,
              permission: Permission.Site_Member_Update,
              title: 'member.manage',
              InjectComponent: SiteMemberEditComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
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
    SiteMemberItemComponent,
    SiteMembersIndexComponent,
    SiteMemberNewComponent,
    SiteMemberEditComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(ROUTES),
    MemberModule,
  ]
})
export class SiteMemberModule {
}

