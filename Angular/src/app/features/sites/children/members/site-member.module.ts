import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PermissionGuard } from 'src/app/core/bia-core/guards/permission.guard';
import { FullPageLayoutComponent } from 'src/app/shared/bia-shared/components/layout/fullpage-layout/fullpage-layout.component';
import { PopupLayoutComponent } from 'src/app/shared/bia-shared/components/layout/popup-layout/popup-layout.component';
import { memberCRUDConfiguration } from 'src/app/shared/bia-shared/feature-templates/members/member.constants';
import { MemberModule } from 'src/app/shared/bia-shared/feature-templates/members/member.module';
import { MemberImportComponent } from 'src/app/shared/bia-shared/feature-templates/members/views/member-import/member-import.component';
import { Permission } from 'src/app/shared/permission';
import { SharedModule } from 'src/app/shared/shared.module';
import { SiteMemberEditComponent } from './views/site-member-edit/site-member-edit.component';
import { SiteMemberItemComponent } from './views/site-member-item/site-member-item.component';
import { SiteMemberNewComponent } from './views/site-member-new/site-member-new.component';
import { SiteMembersIndexComponent } from './views/site-members-index/site-members-index.component';

const ROUTES: Routes = [
  {
    path: '',
    data: {
      breadcrumb: null,
      permission: Permission.Site_Member_List_Access,
      injectComponent: SiteMembersIndexComponent,
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
          injectComponent: SiteMemberNewComponent,
        },
        component: PopupLayoutComponent,
        // component: FullPageLayoutComponent,
        canActivate: [PermissionGuard],
      },
      {
        path: 'import',
        data: {
          breadcrumb: 'member.import',
          canNavigate: false,
          style: {
            minWidth: '80vw',
            maxWidth: '80vw',
            maxHeight: '80vh',
          },
          permission: Permission.Site_Member_Save,
          title: 'member.import',
          injectComponent: MemberImportComponent,
          dynamicComponent: () =>
            memberCRUDConfiguration.usePopup
              ? PopupLayoutComponent
              : FullPageLayoutComponent,
        },
        component: memberCRUDConfiguration.usePopup
          ? PopupLayoutComponent
          : FullPageLayoutComponent,
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
              injectComponent: SiteMemberEditComponent,
            },
            component: PopupLayoutComponent,
            // component: FullPageLayoutComponent,
            canActivate: [PermissionGuard],
          },
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit',
          },
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  declarations: [
    SiteMemberItemComponent,
    SiteMembersIndexComponent,
    SiteMemberNewComponent,
    SiteMemberEditComponent,
  ],
  imports: [SharedModule, RouterModule.forChild(ROUTES), MemberModule],
})
export class SiteMemberModule {}
