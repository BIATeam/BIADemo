import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { AuthService, Permission } from 'packages/bia-ng/core/public-api';
import { DomainUserOptionsStore } from 'packages/bia-ng/domains/public-api';
import { skip } from 'rxjs/operators';
import { CrudItemNewComponent } from '../../../crud-items/views/crud-item-new/crud-item-new.component';
import { MemberFormNewComponent } from '../../components/member-form-new/member-form-new.component';
import { memberCRUDConfiguration } from '../../member.constants';
import { Member, Members } from '../../model/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'bia-member-new',
  templateUrl: './member-new.component.html',
  imports: [MemberFormNewComponent, AsyncPipe],
})
export class MemberNewComponent
  extends CrudItemNewComponent<Member>
  implements OnInit
{
  teamTypeId: number;

  canAddFromDirectory = false;

  public memberService: MemberService;
  protected authService: AuthService;

  public members: Members;

  constructor(protected injector: Injector) {
    super(injector, injector.get<MemberService>(MemberService));
    this.crudConfiguration = memberCRUDConfiguration;
    this.memberService = injector.get<MemberService>(MemberService);
    this.authService = injector.get<AuthService>(AuthService);
  }

  ngOnInit() {
    this.canAddFromDirectory = this.authService.hasPermission(
      Permission.User_Add
    );
    this.crudConfiguration.optionFilter = { teamTypeId: this.teamTypeId };
    this.memberService.teamTypeId = this.teamTypeId;
    this.members = new Members();
    this.sub.add(
      this.store
        .select(DomainUserOptionsStore.getLastUsersAdded)
        .pipe(skip(1))
        .subscribe(lastUsersAdded => {
          this.memberService.optionsService.refreshUsersOptions();
          this.members.users = lastUsersAdded;
        })
    );
    super.ngOnInit();
  }

  onSubmittedMulti(membersToCreate: Members) {
    this.memberService.createMulti(membersToCreate);
    this.members = new Members();
    this.router.navigate(['../'], { relativeTo: this.activatedRoute });
  }
}
