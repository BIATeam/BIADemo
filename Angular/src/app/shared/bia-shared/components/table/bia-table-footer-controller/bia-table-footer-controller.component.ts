import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'bia-table-footer-controller',
  templateUrl: './bia-table-footer-controller.component.html',
  styleUrls: ['./bia-table-footer-controller.component.scss'],
})
export class BiaTableFooterControllerComponent implements OnInit, OnChanges {
  @Input() pageSizeOptions: number[] = [10, 25, 50, 100];
  @Input() pageSize: number;
  @Input() length: number;
  @Input() canChangePageSize = true;
  @Output() pageSizeChange = new EventEmitter<number>();

  pageSizes: SelectItem[];
  resultMessageMapping = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    '=0': 'bia.noResult',
    // eslint-disable-next-line @typescript-eslint/naming-convention
    '=1': 'bia.result',
    other: 'bia.results',
  };

  constructor(public translateService: TranslateService) {}

  ngOnInit() {
    this.updateDisplayedPageSizeOptions();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.pageSizeOptions) {
      this.updateDisplayedPageSizeOptions();
    }
  }

  onPageSizeChange() {
    this.pageSizeChange.emit(Number(this.pageSize));
  }

  protected updateDisplayedPageSizeOptions() {
    if (this.pageSizeOptions) {
      const displayedPageSizeOptions = this.pageSizeOptions.sort(
        (a, b) => a - b
      );

      this.pageSizes = new Array<SelectItem>();
      displayedPageSizeOptions.forEach(displayedPageSizeOption => {
        this.pageSizes.push({
          label: displayedPageSizeOption.toString(),
          value: displayedPageSizeOption,
        });
      });
    }
  }
}
