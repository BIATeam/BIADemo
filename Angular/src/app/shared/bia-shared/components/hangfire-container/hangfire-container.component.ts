import {
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { LayoutHelperService } from '../../services/layout-helper.service';
import { BiaLayoutService } from '../layout/services/layout.service';

@Component({
  selector: 'bia-hangfire-container',
  templateUrl: './hangfire-container.component.html',
  styleUrls: ['./hangfire-container.component.scss'],
  imports: [FormsModule],
})
export class HangfireContainerComponent implements OnInit, OnDestroy {
  @Input() url = '';

  @ViewChild('hangfireForm', { static: false }) hangfireForm: ElementRef;

  protected sub = new Subscription();
  public token = '';

  constructor(
    protected authService: AuthService,
    protected readonly layoutService: BiaLayoutService
  ) {}

  ngOnInit() {
    this.sub.add(
      this.authService.getLightToken().subscribe(authinfo => {
        this.token = authinfo.token;
        setTimeout(() => this.hangfireForm.nativeElement.submit());
      })
    );
  }

  ngOnDestroy() {
    // remove cookie HangFireCookie
    document.cookie =
      'HangFireCookie=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }

  getIFrameHeight(): string {
    return `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService, ' + 3.5rem')})`;
  }
}
