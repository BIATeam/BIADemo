import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { User } from 'src/app/domains/user/model/user';
import { Role } from 'src/app/domains/role/model/role';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { TranslateService } from '@ngx-translate/core';
import { Site } from '../../../../model/site/site';
import { Member } from '../../model/member';
import { MemberRole } from '../../model/member-role';

export interface MemberNewFormVM {
  users: User[];
  roles: Role[] | null;
}

@Component({
  selector: 'app-member-new-form',
  templateUrl: './member-new-form.component.html',
  styleUrls: ['./member-new-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MemberNewFormComponent implements OnInit, OnChanges {
  @Input() allRoles: Role[];
  @Input() site: Site;
  @Input() users: User[];

  @Output() searchUsers = new EventEmitter<string>();
  @Output() save = new EventEmitter<Member[]>();
  @Output() cancel = new EventEmitter();

  selecteds: User[];
  memberNewFormData = <MemberNewFormVM>{};
  form: FormGroup;

  constructor(
    public formBuilder: FormBuilder,
    public translateService: TranslateService,
    private biaMessageService: BiaMessageService
  ) {
    this.initForm();
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (this.users && changes.users) {
      this.users = this.users.sort((a, b) => {
        return a.firstName.localeCompare(b.firstName);
      });
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      users: [this.memberNewFormData.users, Validators.required],
      roles: [this.memberNewFormData.roles]
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      this.createMember();
      this.form.reset();
    }
  }

  onSearchUsers(event: any) {
    this.searchUsers.emit(event.query);
  }

  onSelect(userSelected: User) {
    if (userSelected && userSelected.siteIds && userSelected.siteIds.indexOf(this.site.id) > -1) {
      this.selecteds.splice(this.selecteds.map((x) => x.id).indexOf(userSelected.id), 1);
      this.biaMessageService.showInfo(this.translateService.instant('biaMsg.userAlreadyAssociatedSite'));
    }
  }

  private createMember() {
    const memberToCreates = new Array<Member>();
    this.form.value.users.forEach((userSelected: User) => {
      const memberToCreate: Member = {
        id: 0,
        userId: userSelected.id,
        userFirstName: userSelected.firstName,
        userLastName: userSelected.lastName,
        userLogin: userSelected.login,
        siteId: this.site.id,
        roles: this.rolesToMemberRoles(this.form.value.roles),
        dtoState: DtoState.Added
      };
      memberToCreates.push(memberToCreate);
    });
    this.save.emit(memberToCreates);
  }

  private rolesToMemberRoles(roles: Role[]): MemberRole[] {
    const memberRoles = new Array<MemberRole>();
    if (roles && roles.length > 0) {
      roles.forEach((role) => {
        const memberRole: MemberRole = { id: 0, memberId: 0, roleId: role.id, dtoState: DtoState.Added };
        memberRoles.push(memberRole);
      });
    }
    return memberRoles;
  }
}
