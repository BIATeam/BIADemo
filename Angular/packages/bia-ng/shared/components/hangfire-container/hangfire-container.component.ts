import {
  Component,
  DestroyRef,
  ElementRef,
  effect,
  inject,
  input,
  viewChild,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@bia-team/bia-ng/core';
import { map } from 'rxjs';
import { LayoutHelperService } from '../../services/layout-helper.service';
import { BiaLayoutService } from '../layout/services/layout.service';

@Component({
  selector: 'bia-hangfire-container',
  templateUrl: './hangfire-container.component.html',
  styleUrls: ['./hangfire-container.component.scss'],
})
export class HangfireContainerComponent {
  readonly url = input('');

  private readonly authService = inject(AuthService);
  protected readonly layoutService = inject(BiaLayoutService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly iframeEl =
    viewChild<ElementRef<HTMLIFrameElement>>('iframeEl');

  private readonly token = toSignal(
    this.authService.getLightToken().pipe(map(info => info.token))
  );

  constructor() {
    effect(() => {
      const token = this.token();
      const iframe = this.iframeEl();
      if (!token || !iframe) return;

      const form = document.createElement('form');
      form.method = 'POST';
      form.action = this.url();
      form.target = iframe.nativeElement.name;
      form.style.display = 'none';

      const input = document.createElement('input');
      input.type = 'hidden';
      input.name = 'jwt_token';
      input.value = token;
      form.appendChild(input);

      document.body.appendChild(form);
      form.submit();
      document.body.removeChild(form);
    });

    this.destroyRef.onDestroy(() => {
      document.cookie =
        'HangFireCookie=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    });
  }

  protected getIFrameHeight(): string {
    return `calc(${LayoutHelperService.defaultContainerHeight(this.layoutService, ' + 3.5rem')})`;
  }
}
