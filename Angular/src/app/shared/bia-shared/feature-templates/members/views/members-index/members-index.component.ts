import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CrudItemsIndexComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-items-index/crud-items-index.component';
import { MemberTableComponent } from '../../components/member-table/member-table.component';
import { memberCRUDConfiguration } from '../../member.constants';
import { Member } from '../../model/member';
import { MemberService } from '../../services/member.service';

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
    this.pageSize = 100;
  }

  ngOnInit() {
    this.crudConfiguration.optionFilter = { teamTypeId: this.teamTypeId };
    this.memberService.teamTypeId = this.teamTypeId;
    super.ngOnInit();
  }
}
