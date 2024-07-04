import { Component, Injector, OnInit } from '@angular/core';
import { Member, Members } from '../../model/member';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { MemberService } from '../../services/member.service';
import { memberCRUDConfiguration } from '../../member.constants';
import { Permission } from 'src/app/shared/permission';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { getLastUsersAdded } from 'src/app/domains/bia-domains/user-option/store/user-option.state';
import { skip } from 'rxjs/operators';

@Component({
  selector: 'bia-member-new',
  templateUrl: './member-new.component.html',
})
export class MemberNewComponent
  extends CrudItemNewComponent<Member>
  implements OnInit
{
  teamTypeId: number;

  canAddFromDirectory = false;

  public memberService: MemberService;
  private authService: AuthService;

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
        .select(getLastUsersAdded)
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
