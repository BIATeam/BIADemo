import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { BiaPermission } from 'packages/bia-ng/core/public-api';
import { SpinnerComponent } from '../../../../components/spinner/spinner.component';
import { CrudItemEditComponent } from '../../../crud-items/views/crud-item-edit/crud-item-edit.component';
import { MemberFormEditComponent } from '../../components/member-form-edit/member-form-edit.component';
import { memberCRUDConfiguration } from '../../member.constants';
import { Member } from '../../model/member';
import { MemberService } from '../../services/member.service';

@Component({
  selector: 'bia-member-edit',
  templateUrl: './member-edit.component.html',
  imports: [MemberFormEditComponent, SpinnerComponent, AsyncPipe],
})
export class MemberEditComponent
  extends CrudItemEditComponent<Member>
  implements OnInit
{
  teamTypeId: number;
  canAddFromDirectory = false;

  public memberService: MemberService;
  constructor(protected injector: Injector) {
    super(injector, injector.get<MemberService>(MemberService));
    this.crudConfiguration = memberCRUDConfiguration;
    this.memberService = injector.get<MemberService>(MemberService);
  }

  ngOnInit() {
    this.canAddFromDirectory = this.authService.hasPermission(
      BiaPermission.User_Add
    );
    this.crudConfiguration.optionFilter = { teamTypeId: this.teamTypeId };
    this.memberService.teamTypeId = this.teamTypeId;
    super.ngOnInit();
  }
}
