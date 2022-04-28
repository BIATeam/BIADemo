import { Component, OnChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { BiaCalcTableComponent } from 'src/app/shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component';
import { Member } from '../../model/member';

@Component({
  selector: 'bia-member-table',
  templateUrl: '../../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/components/table/bia-calc-table/bia-calc-table.component.scss']
})
export class MemberTableComponent extends BiaCalcTableComponent implements OnChanges {

  constructor(
    public formBuilder: FormBuilder,
    public authService: AuthService,
    public biaMessageService: BiaMessageService,
    public translateService: TranslateService,
    public siteService: SiteService,
  ) {
    super(formBuilder, authService, biaMessageService, translateService);
  }

  public initForm() {
    this.form = this.formBuilder.group({
      id: [this.element.id], // This field is mandatory. Do not remove it.
      user: [this.element.user, Validators.required],
      roles: [this.element.roles],
    });
  }

    onSubmit() {
    if (this.form.valid) {
      const member: Member = <Member>this.form.value;
      member.id = member.id > 0 ? member.id : 0;
      member.roles = BiaOptionService.Differential(member.roles, this.element?.roles);
      member.user = {...member.user};

      // force the parent key => siteId from authService or other Id from 'parent'Service
      member.teamId = this.siteService.currentSiteId;
      this.save.emit(member);
      this.form.reset();
    }
  }
}
