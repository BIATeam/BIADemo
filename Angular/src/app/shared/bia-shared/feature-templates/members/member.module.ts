import { NgModule } from '@angular/core';
// import { ReducerManager, StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { RoleOptionModule } from 'src/app/domains/bia-domains/role-option/role-option.module';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';
import { UserFromDirectoryModule } from 'src/app/features/bia-features/users-from-directory/user-from-directory.module';

import { MemberFormEditComponent } from './components/member-form-edit/member-form-edit.component';
import { MemberFormNewComponent } from './components/member-form-new/member-form-new.component';
import { MemberFormComponent } from './components/member-form/member-form.component';
import { MemberTableComponent } from './components/member-table/member-table.component';
import { memberCRUDConfiguration } from './member.constants';
import { FeatureMembersStore } from './store/member.state';
import { MembersEffects } from './store/members-effects';
import { MemberEditComponent } from './views/member-edit/member-edit.component';
import { MemberImportComponent } from './views/member-import/member-import.component';
import { MemberItemComponent } from './views/member-item/member-item.component';
import { MemberNewComponent } from './views/member-new/member-new.component';
import { MembersIndexComponent } from './views/members-index/members-index.component';

@NgModule({
  imports: [
    // RouterModule.forChild(ROUTES),
    StoreModule.forFeature(
      memberCRUDConfiguration.storeKey,
      FeatureMembersStore.reducers
    ),
    EffectsModule.forFeature([MembersEffects]),
    // TODO after creation of CRUD Member : select the optioDto dommain module required for link
    // Domain Modules:
    UserOptionModule,
    RoleOptionModule,
    UserFromDirectoryModule,
    MemberItemComponent,
    MembersIndexComponent,
    // [Calc] : NOT used for calc (3 lines).
    // it is possible to delete unsed commponent files (views/..-new + views/..-edit + components/...-form).
    MemberFormComponent,
    MemberFormEditComponent,
    MemberFormNewComponent,
    MemberNewComponent,
    MemberEditComponent,
    // [Calc] : Used only for calc it is possible to delete unsed commponent files (components/...-table)).
    MemberTableComponent,
    MemberImportComponent,
  ],
  exports: [
    MemberItemComponent,
    MemberFormComponent,
    MemberFormEditComponent,
    MemberFormNewComponent,
    MembersIndexComponent,
    MemberNewComponent,
    MemberEditComponent,
    MemberTableComponent,
  ],
})
export class MemberModule {}
