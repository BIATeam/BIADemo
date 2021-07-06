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
import { FormBuilder, FormGroup } from '@angular/forms';
import { Member } from '../../model/member';
import { MemberRole } from '../../model/member-role';
import { DtoState } from 'src/app/shared/bia-shared/model/dto-state.enum';
import { Role } from 'src/app/domains/role/model/role';
import { TranslateService } from '@ngx-translate/core';

export interface MemberEditFormVM {
  roles: Role[] | null;
}

@Component({
  selector: 'app-member-edit-form',
  templateUrl: './member-edit-form.component.html',
  styleUrls: ['./member-edit-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MemberEditFormComponent implements OnInit, OnChanges {
  @Input() member: Member = <Member>{};
  @Input() allRoles: Role[];

  @Output() save = new EventEmitter<Member>();
  @Output() cancel = new EventEmitter();

  memberEditFormData = <MemberEditFormVM>{};
  form: FormGroup;

  constructor(public formBuilder: FormBuilder, public translateService: TranslateService) {
    this.initForm();
  }

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (this.allRoles && this.member && (changes.member || changes.allRoles)) {
      const vm: MemberEditFormVM = {
        roles: this.member.roles ? this.allRoles.filter((x) => this.member.roles.some((y) => y.roleId === x.id)) : []
      };
      this.form.patchValue({ ...vm });
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      roles: [this.memberEditFormData.roles]
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      this.updateMember();
      this.form.reset();
    }
  }

  private updateMember() {
    const memberToUpdate: Member = { ...this.member };
    const formRoles: Role[] = this.form.value.roles;

    memberToUpdate.roles.forEach((currentRole) => {
      if (!formRoles.some((x) => x.id === currentRole.roleId)) {
        currentRole.dtoState = DtoState.Deleted;
      }
    });

    formRoles.forEach((formRole: Role) => {
      if (!memberToUpdate.roles.some((x) => x.roleId === formRole.id)) {
        const memberRole: MemberRole = {
          id: 0,
          memberId: memberToUpdate.id,
          roleId: formRole.id,
          dtoState: DtoState.Added
        };
        memberToUpdate.roles.push(memberRole);
      }
    });
    this.save.emit(memberToUpdate);
  }
}
