import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { HistoricEntryDto } from 'packages/bia-ng/models/dto/historic-entry-dto';
import { TimelineModule } from 'primeng/timeline';

@Component({
  selector: 'bia-crud-item-historic',
  imports: [TimelineModule, DatePipe],
  templateUrl: './crud-item-historic.component.html',
  styleUrl: './crud-item-historic.component.scss',
})
export class CrudItemHistoricComponent {
  @Input() historicEntries: HistoricEntryDto[] = [];
}
