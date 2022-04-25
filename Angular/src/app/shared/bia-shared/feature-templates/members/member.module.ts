import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { MembersEffects } from './store/members-effects';
import { reducers } from './store/member.state';
import { MemberFormComponent } from './components/member-form/member-form.component';
import { MembersIndexComponent } from './views/members-index/members-index.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MemberNewComponent } from './views/member-new/member-new.component';
import { MemberEditComponent } from './views/member-edit/member-edit.component';
import { MemberItemComponent } from './views/member-item/member-item.component';
import { MemberTableComponent } from './components/member-table/member-table.component';
import { RoleOptionModule } from 'src/app/domains/bia-domains/role-option/role-option.module';
import { UserOptionModule } from 'src/app/domains/bia-domains/user-option/user-option.module';
import { UserFromDirectoryModule } from 'src/app/features/bia-features/users-from-directory/user-from-directory.module';

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
    StoreModule.forFeature('members', reducers),
    EffectsModule.forFeature([MembersEffects]),
    // Domain Modules:
    UserOptionModule,
    RoleOptionModule,
    UserFromDirectoryModule, // requiered for the add user from directory feature
  ],
  exports: [
    MemberItemComponent,
    MemberFormComponent,
    MembersIndexComponent,
    MemberNewComponent,
    MemberEditComponent,
    MemberTableComponent,
  ]
})
export class MemberModule {
}

