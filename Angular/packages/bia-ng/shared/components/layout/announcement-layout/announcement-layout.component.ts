import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { BiaAppConstantsService } from 'packages/bia-ng/core/public-api';
import { BiaAnnouncementType } from 'packages/bia-ng/models/enum/public-api';
import { Announcement } from 'packages/bia-ng/models/public-api';
import { SafeHtmlPipe } from '../../../pipes/safe-html.pipe';

interface FormattedAnnouncement {
  html: string;
  type: any;
  key: string | number;
}

@Component({
  selector: 'bia-announcement-layout',
  imports: [CommonModule, SafeHtmlPipe, TranslateModule],
  templateUrl: './announcement-layout.component.html',
  styleUrls: ['./announcement-layout.component.scss'],
})
export class AnnouncementLayoutComponent implements OnChanges, OnDestroy {
  @Input() announcements: Announcement[] | null;

  displayDurationMs = BiaAppConstantsService.announcementDisplayDurationMs;
  transitionDurationMs = 500;

  formattedAnnouncements: FormattedAnnouncement[] = [];
  currentFormatedAnnouncementIndex = 0;
  currentFormatedAnnouncement: FormattedAnnouncement | null = null;

  private animationTimeoutId: number | null = null;
  private currentPhase: 'enter' | 'stay' | 'exit' = 'enter';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.announcements && this.announcements) {
      this.formattedAnnouncements = this.announcements.map(announcement => {
        return {
          html:
            this.buildIconElement(announcement.type.id) +
            announcement.rawContent,
          type: announcement.type,
          key: announcement.id,
        };
      });

      if (this.formattedAnnouncements.length === 0) {
        this.clearTimeout();
        this.currentFormatedAnnouncement = null;
        this.currentFormatedAnnouncementIndex = 0;
        return;
      }

      if (this.currentFormatedAnnouncement) {
        const currentKey = this.currentFormatedAnnouncement.key;
        const foundIndex = this.formattedAnnouncements.findIndex(
          announcement => announcement.key === currentKey
        );

        if (foundIndex !== -1) {
          this.currentFormatedAnnouncementIndex = foundIndex;
        } else {
          const nextIndex = Math.min(
            this.currentFormatedAnnouncementIndex,
            this.formattedAnnouncements.length - 1
          );
          this.currentFormatedAnnouncementIndex = nextIndex;
          this.currentPhase = 'stay';
        }

        this.currentFormatedAnnouncement =
          this.formattedAnnouncements[this.currentFormatedAnnouncementIndex];
        this.ensureSequenceRunning();
        return;
      }

      this.currentFormatedAnnouncementIndex = 0;
      this.startSequence();
    }
  }

  ngOnDestroy(): void {
    this.clearTimeout();
  }

  private buildIconElement(typeId: number): string {
    switch (typeId) {
      case BiaAnnouncementType.information:
        return '<i class="pi pi-info-circle announcement-icon-info"></i>';
      case BiaAnnouncementType.warning:
        return '<i class="pi pi-exclamation-triangle announcement-icon-warning"></i>';
      default:
        return '';
    }
  }

  private startSequence(): void {
    if (
      !this.formattedAnnouncements ||
      this.formattedAnnouncements.length === 0
    ) {
      return;
    }

    this.currentPhase = 'enter';
    this.currentFormatedAnnouncement =
      this.formattedAnnouncements[this.currentFormatedAnnouncementIndex];
    this.scheduleNextPhase(this.transitionDurationMs);
  }

  private ensureSequenceRunning(): void {
    if (this.formattedAnnouncements.length === 1) {
      this.currentPhase = 'stay';
      this.clearTimeout();
      return;
    }

    if (this.currentPhase === 'stay') {
      this.scheduleNextPhase(this.displayDurationMs);
    } else {
      this.scheduleNextPhase(this.transitionDurationMs);
    }
  }

  private scheduleNextPhase(delayMs: number): void {
    this.clearTimeout();

    this.animationTimeoutId = window.setTimeout(() => {
      if (this.currentPhase === 'enter') {
        this.currentPhase = 'stay';
        if (this.formattedAnnouncements.length === 1) {
          return;
        }

        this.scheduleNextPhase(this.displayDurationMs);
      } else if (this.currentPhase === 'stay') {
        this.currentPhase = 'exit';
        this.scheduleNextPhase(this.transitionDurationMs);
      } else if (this.currentPhase === 'exit') {
        this.currentFormatedAnnouncementIndex++;

        if (
          this.currentFormatedAnnouncementIndex >=
          this.formattedAnnouncements.length
        ) {
          this.currentFormatedAnnouncementIndex = 0;
        }

        this.startSequence();
      }
    }, delayMs);
  }

  private clearTimeout(): void {
    if (this.animationTimeoutId !== null) {
      clearTimeout(this.animationTimeoutId);
      this.animationTimeoutId = null;
    }
  }

  getAnimationClass(): string {
    return `announcement-${this.currentPhase}`;
  }
}
