import {
  Component,
  ElementRef,
  HostListener,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { LayoutHelperService } from '../../services/layout-helper.service';
import { BiaLayoutService } from '../layout/services/layout.service';

@Component({
  selector: 'bia-hangfire-container',
  templateUrl: './hangfire-container.component.html',
  styleUrls: ['./hangfire-container.component.scss'],
})
export class HangfireContainerComponent implements OnInit, OnDestroy {
  @Input() url = '';
  iFrameHeight: string;

  // @ViewChild('iFrame2', { static: false }) iFrame2: ElementRef;
  @ViewChild('hangfireForm', { static: false }) hangfireForm: ElementRef;

  protected sub = new Subscription();
  public token = '';

  constructor(
    protected authService: AuthService,
    protected readonly layoutService: BiaLayoutService
  ) {
    this.getIFrameHeight();
  }

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

  @HostListener('window:resize', ['$event'])
  getIFrameHeight() {
    this.iFrameHeight = `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService)})`;
  }
}
