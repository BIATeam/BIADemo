import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { Announcement } from 'packages/bia-ng/features/public-api';
import { BiaAnnouncementType } from 'packages/bia-ng/models/enum/bia-announcement-type.enum';
import { SafeHtmlPipe } from '../../../pipes/safe-html.pipe';

@Component({
  selector: 'bia-announcement-layout',
  imports: [SafeHtmlPipe, TranslateModule],
  templateUrl: './announcement-layout.component.html',
  styleUrl: './announcement-layout.component.scss',
})
export class AnnouncementLayoutComponent
  implements OnChanges, AfterViewInit, OnDestroy
{
  @Input() messages: Announcement[] | null = null;

  @ViewChild('contentContainer', { static: false })
  contentContainer!: ElementRef<HTMLDivElement>;

  @ViewChild('scrollContent', { static: false })
  scrollContent!: ElementRef<HTMLDivElement>;

  formatedMessages = '';

  private readonly speedPxPerSec = 40;

  private viewReady = false;

  private containerWidth = 0;
  private contentWidth = 0;

  private currentX = 0;
  private lastTimestamp: number | null = null;
  private animationFrameId: number | null = null;

  private pendingResizeAdjust = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.messages) {
      this.formatedMessages = (this.messages ?? [])
        .map(message => {
          let iconElement = '';
          switch (message.type.id) {
            case BiaAnnouncementType.Info:
              iconElement =
                '<i class="pi pi-info-circle announcement-icon-info"></i>';
              break;
            case BiaAnnouncementType.Warning:
              iconElement =
                '<i class="pi pi-exclamation-triangle announcement-icon-warning"></i>';
              break;
          }
          return iconElement + message.rawContent;
        })
        .join('<span class="announcement-separator">|</span>');

      this.pendingResizeAdjust = false;
      this.scheduleMeasure();
    }
  }

  ngAfterViewInit(): void {
    this.viewReady = true;
    this.scheduleMeasure();
    this.startAnimationLoop();
  }

  ngOnDestroy(): void {
    if (this.animationFrameId !== null) {
      cancelAnimationFrame(this.animationFrameId);
    }
  }

  @HostListener('window:resize')
  onResize(): void {
    this.pendingResizeAdjust = true;
    this.scheduleMeasure();
  }

  private scheduleMeasure(): void {
    if (!this.viewReady) return;

    requestAnimationFrame(() => this.measureSizes());
  }

  /**
   * Measures the widths of the scrolling container and content, and adjusts
   * the current scroll position accordingly.
   *
   * Responsibilities:
   * - On first run: initializes sizes and starts the text just outside the right edge.
   * - On window resize: preserves the relative scroll progress (percentage of the path).
   * - On text/content change: keeps current position when possible, and only resets
   *   if the text is fully out of view on the left.
   */
  private measureSizes(): void {
    const containerEl = this.contentContainer?.nativeElement;
    const contentEl = this.scrollContent?.nativeElement;
    if (!containerEl || !contentEl) return;

    // Store old values so we can compute the relative progress in case of resize
    const oldContainerWidth = this.containerWidth;
    const oldContentWidth = this.contentWidth;
    const oldCurrentX = this.currentX;

    // New measured sizes from the DOM
    const newContainerWidth = containerEl.offsetWidth;
    const newContentWidth = contentEl.scrollWidth;

    // If measurements are invalid (0 or NaN), just store and abort this cycle
    if (!newContainerWidth || !newContentWidth) {
      this.containerWidth = newContainerWidth;
      this.contentWidth = newContentWidth;
      return;
    }

    /**
     * CASE 1: First initialization
     *
     * If we didn't have valid old sizes or currentX is still 0, we consider this
     * as the initial setup. We place the text just outside the right edge of the
     * container so that it scrolls into view.
     */
    if (!oldContainerWidth || !oldContentWidth || this.currentX === 0) {
      this.containerWidth = newContainerWidth;
      this.contentWidth = newContentWidth;

      // Start just outside the right side of the visible area
      this.currentX = this.containerWidth;
      this.applyTransform();
      return;
    }

    // Update stored sizes with the new measured values
    this.containerWidth = newContainerWidth;
    this.contentWidth = newContentWidth;

    /**
     * CASE 2: Adjust after a window resize
     *
     * We want to preserve the current "progress" of the scrolling animation,
     * not the absolute pixel position. This means:
     *
     *   - Before resize: the text followed a path from startOld to endOld
     *   - After resize: the path changes (startNew / endNew), but we keep the same
     *     relative position s in [0, 1).
     */
    if (this.pendingResizeAdjust) {
      this.pendingResizeAdjust = false;

      const startOld = oldContainerWidth;
      const totalOld = oldContainerWidth + oldContentWidth; // full path length (right -> left)

      if (totalOld > 0) {
        // How many pixels have we already travelled from the old starting point?
        const distanceOld = startOld - oldCurrentX;

        // Normalized progress along the old path, in [0, 1)
        let s = distanceOld / totalOld;
        s = s - Math.floor(s); // ensure s stays within [0, 1) even if slightly off

        const startNew = this.containerWidth;
        const totalNew = this.containerWidth + this.contentWidth;

        // Rebuild currentX so that we keep the same relative progress on the new path
        this.currentX = startNew - s * totalNew;
      }
    } else {
      /**
       * CASE 3: Content change or other re-measure (not a resize)
       *
       * We keep the currentX as is, except if the text is *completely*
       * out of view on the left with the new sizes. In that case, we
       * restart it at the right edge to avoid being "lost" off-screen.
       */
      if (this.currentX < -this.contentWidth) {
        this.currentX = this.containerWidth;
      }
    }

    // Apply the updated position to the DOM
    this.applyTransform();
  }

  /**
   * Starts the main animation loop using requestAnimationFrame.
   *
   * Responsibilities:
   * - Advances the text horizontally based on real elapsed time (delta time),
   *   ensuring a constant speed in px/s regardless of frame rate.
   * - Wraps the text back to the right side when it has fully scrolled out
   *   of view on the left.
   *
   * This loop runs for the lifetime of the component, until ngOnDestroy cancels it.
   */
  private startAnimationLoop(): void {
    /**
     * One animation step, called before each repaint by requestAnimationFrame.
     *
     * @param timestamp High-resolution time (ms) since page load, provided by the browser.
     */
    const step = (timestamp: number) => {
      // If we don't yet know the sizes, we can't scroll meaningfully.
      // Wait until measureSizes has populated containerWidth/contentWidth.
      if (!this.contentWidth || !this.containerWidth) {
        this.animationFrameId = requestAnimationFrame(step);
        return;
      }

      // First frame: initialize lastTimestamp
      if (this.lastTimestamp === null) {
        this.lastTimestamp = timestamp;
      }

      // Compute elapsed time between two frames in seconds
      const deltaSec = (timestamp - this.lastTimestamp) / 1000;
      this.lastTimestamp = timestamp;

      // Move the text to the left by (speed * time)
      this.currentX -= this.speedPxPerSec * deltaSec;

      /**
       * If the text has completely left the screen on the left side
       * (its rightmost pixel is <= left edge), we wrap it back to
       * just outside the right edge.
       */
      if (this.currentX <= -this.contentWidth) {
        this.currentX = this.containerWidth;
      }

      // Apply the new position to the DOM element
      this.applyTransform();

      // Schedule the next frame of the animation
      this.animationFrameId = requestAnimationFrame(step);
    };

    // Kick off the animation loop
    this.animationFrameId = requestAnimationFrame(step);
  }

  private applyTransform(): void {
    const contentEl = this.scrollContent?.nativeElement;
    if (!contentEl) return;

    contentEl.style.transform = `translateX(${this.currentX}px)`;
  }
}
