import { Component, Injector, OnInit } from '@angular/core';
import { Member } from '../../model/member';
import { memberCRUDConfiguration } from '../../member.constants';
import { CrudItemEditComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-edit/crud-item-edit.component';
import { MemberService } from '../../services/member.service';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { Permission } from 'src/app/shared/permission';

@Component({
  selector: 'bia-member-edit',
  templateUrl: './member-edit.component.html',
})
export class MemberEditComponent
  extends CrudItemEditComponent<Member>
  implements OnInit
{
  teamTypeId: number;
  canAddFromDirectory = false;

  public memberService: MemberService;
  protected authService: AuthService;
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
    super.ngOnInit();
  }
}
