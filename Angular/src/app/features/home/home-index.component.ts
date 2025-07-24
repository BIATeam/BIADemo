import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
  imports: [TranslateModule],
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  @ViewChild('iframe') iframeRef: ElementRef<HTMLIFrameElement>;

  isIframeReady = false;

  constructor(private layoutService: BiaLayoutService) {}

  ngOnInit(): void {
    this.layoutService.hideBreadcrumb();
    window.addEventListener('message', this.onMessage.bind(this));
  }

  ngOnDestroy(): void {
    this.layoutService.showBreadcrumb();
    window.removeEventListener('message', this.onMessage.bind(this));
  }

  sendTestMessage() {
    this.iframeRef.nativeElement?.contentWindow?.postMessage(
      {
        type: 'TEST',
        message: 'This is a test message',
      },
      'http://localhost:4201'
    );
  }

  onMessage(event: MessageEvent): void {
    if (event.data.type === 'IFRAME_READY') {
      this.isIframeReady = true;
    }
  }
}
