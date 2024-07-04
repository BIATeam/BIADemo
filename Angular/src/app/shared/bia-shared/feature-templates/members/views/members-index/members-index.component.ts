import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../model/member';
import { memberCRUDConfiguration } from '../../member.constants';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { MemberService } from '../../services/member.service';
import { MemberTableComponent } from '../../components/member-table/member-table.component';

@Component({
  selector: 'bia-members-index',
  templateUrl: './members-index.component.html',
  styleUrls: ['./members-index.component.scss'],
})
export class MembersIndexComponent
  extends CrudItemsIndexComponent<Member>
  implements OnInit
{
  teamTypeId: number;
  memberService: MemberService;
  @ViewChild(MemberTableComponent, { static: false })
  crudItemTableComponent: MemberTableComponent;
  constructor(protected injector: Injector) {
    super(injector, injector.get<MemberService>(MemberService));
    this.crudConfiguration = memberCRUDConfiguration;
    this.memberService = injector.get<MemberService>(MemberService);
  }

  ngOnInit() {
    this.crudConfiguration.optionFilter = { teamTypeId: this.teamTypeId };
    this.memberService.teamTypeId = this.teamTypeId;
    super.ngOnInit();
  }
}
