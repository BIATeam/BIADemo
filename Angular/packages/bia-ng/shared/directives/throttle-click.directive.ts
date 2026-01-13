import {
  Directive,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { throttleTime } from 'rxjs/operators';

@Directive({
  selector: '[biaThrottleEvent]',
  exportAs: 'biaThrottleEvent',
  standalone: true,
})
export class ThrottleEventDirective implements OnInit, OnDestroy {
  @Input() throttleTime = 1000; // Default: 1 second
  @Output() throttledEvent = new EventEmitter<any>();

  private eventSubject = new Subject<any>();
  private subscription!: Subscription;

  ngOnInit() {
    this.subscription = this.eventSubject
      .pipe(throttleTime(this.throttleTime))
      .subscribe(event => {
        this.throttledEvent.emit(event);
      });
  }

  public forwardEvent(event: any) {
    this.eventSubject.next(event);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
