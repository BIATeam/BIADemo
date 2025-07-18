import { animate, style, transition, trigger } from '@angular/animations';
import { NgFor, NgIf } from '@angular/common';
import {
  AfterViewInit,
  Component,
  effect,
  ElementRef,
  HostBinding,
  Input,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { MenuItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { BiaLayoutService } from '../../services/layout.service';
import { BiaUltimaMenuItemComponent } from '../menu-item/ultima-menu-item.component';

@Component({
  selector: 'bia-ultima-menu',
  templateUrl: './ultima-menu.component.html',
  styleUrls: ['./ultima-menu.component.scss'],
  animations: [
    trigger('menu', [
      transition('void => inline', [
        style({ height: 0 }),
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 1, height: '*' })
        ),
      ]),
      transition('inline => void', [
        animate(
          '400ms cubic-bezier(0.86, 0, 0.07, 1)',
          style({ opacity: 0, height: '0' })
        ),
      ]),
      transition('void => overlay', [
        style({ opacity: 0, transform: 'scaleY(0.8)' }),
        animate('.12s cubic-bezier(0, 0, 0.2, 1)'),
      ]),
      transition('overlay => void', [
        animate('.1s linear', style({ opacity: 0 })),
      ]),
    ]),
  ],
  imports: [NgFor, NgIf, BiaUltimaMenuItemComponent, ButtonModule],
})
export class BiaUltimaMenuComponent implements AfterViewInit, OnDestroy {
  @HostBinding('class') classes = 'bia-ultima-menu';

  @Input() menuItems: MenuItem[] = [];

  @ViewChild('scrollContent') scrollContent: ElementRef;

  scrollInterval: NodeJS.Timeout | null;
  isScrollable = false;
  isAtStart = false;
  isAtEnd = false;

  constructor(private readonly layoutService: BiaLayoutService) {
    effect(() => {
      this.layoutService.config();
      this.checkScrollable();
    });
  }

  ngAfterViewInit() {
    this.checkScrollable();
    this.updateButtonState();
    window.addEventListener('resize', () => {
      this.checkScrollable();
      this.updateButtonState();
    });
  }

  checkScrollable() {
    if (this.scrollContent) {
      const content = this.scrollContent.nativeElement;
      this.isScrollable =
        this.layoutService.isHorizontal() &&
        content.scrollWidth > content.clientWidth;
    }
  }

  updateButtonState() {
    if (this.scrollContent) {
      const content = this.scrollContent.nativeElement;
      this.isAtStart = content.scrollLeft <= 0;
      this.isAtEnd =
        content.scrollLeft + content.clientWidth >= content.scrollWidth;
    }
  }

  onScroll() {
    this.updateButtonState();
  }

  startScrolling(event: MouseEvent, direction: 'left' | 'right') {
    event.preventDefault();
    const step = direction === 'left' ? -200 : 200;
    this.scrollContent.nativeElement.scrollBy({
      left: step,
      behavior: 'smooth',
    });
    setTimeout(() => this.updateButtonState(), 300);
    this.scrollInterval = setInterval(() => {
      this.scrollContent.nativeElement.scrollBy({
        left: step,
        behavior: 'smooth',
      });
      setTimeout(() => this.updateButtonState(), 300);
    }, 500);
  }

  stopScrolling() {
    if (this.scrollInterval) {
      clearInterval(this.scrollInterval);
      this.scrollInterval = null;
    }
  }

  ngOnDestroy() {
    this.stopScrolling();
    window.removeEventListener('resize', () => {
      this.checkScrollable();
      this.updateButtonState();
    });
  }
}
