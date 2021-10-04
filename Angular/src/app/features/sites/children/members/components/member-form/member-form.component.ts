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
import { TranslateService } from '@ngx-translate/core';
import { BiaOptionService } from 'src/app/core/bia-core/services/bia-option.service';
import { SiteService } from 'src/app/features/sites/services/site.service';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';
import { Member } from '../../model/member';

@Component({
  selector: 'app-member-form',
  templateUrl: './member-form.component.html',
  styleUrls: ['./member-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})

export class MemberFormComponent implements OnInit, OnChanges {
  @Input() member: Member = <Member>{};
  @Input() roleOptions: OptionDto[];
  @Input() userOptions: OptionDto[];

  @Output() save = new EventEmitter<Member>();
  @Output() cancel = new EventEmitter();

  form: FormGroup;

  constructor(
    public formBuilder: FormBuilder, 
    public translateService : TranslateService,
    public siteService: SiteService,) {
    this.initForm();
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.member) {
      this.form.reset();
      if (this.member) {
        this.form.patchValue({ ...this.member });
      }
    }
  }

  private initForm() {
    this.form = this.formBuilder.group({
      id: [this.member.id],
      user: [this.member.user, Validators.required],
      roles: [this.member.roles],
    });
  }

  onCancel() {
    this.form.reset();
    this.cancel.next();
  }

  onSubmit() {
    if (this.form.valid) {
      const member: Member = <Member>this.form.value;
      member.id = member.id > 0 ? member.id : 0;
      member.roles = BiaOptionService.Differential(member.roles, this.member?.roles);
      member.user = {...member.user}

      // force the parent key => siteId from authService or other Id from 'parent'Service
      member.siteId = this.siteService.currentSiteId;
      this.save.emit(member);
      this.form.reset();
    }
  }
}

