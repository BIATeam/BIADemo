import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { View } from '../../model/view';
import { AssignViewToSite } from '../../model/assign-view-to-site';
import { ViewSite } from '../../model/view-site';
import { Site } from 'src/app/domains/site/model/site';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-view-site-table',
  templateUrl: './view-site-table.component.html',
  styleUrls: ['./view-site-table.component.scss']
})
export class ViewSiteTableComponent implements OnChanges {
  @Input() views: View[];
  @Input() siteSelected: Site;
  @Input() canDelete = false;
  @Input() canSetDefault = false;
  @Input() canUpdate = false;
  @Input() canAssign = false;

  get viewSelected(): View {
    if (this.table) {
      return this.table.selection as View;
    }
    return {} as View;
  }
  set viewSelected(value: View) {
    if (this.table) {
      this.table.selection = value;
    }
  }

  @ViewChild('viewSiteTable', { static: false }) table: Table;

  @Output() assignViewToSite = new EventEmitter<AssignViewToSite>();
  @Output() delete = new EventEmitter<number>();
  @Output() setDefault = new EventEmitter<{ viewId: number; isDefault: boolean }>();
  @Output() viewSelect = new EventEmitter<View>();

  constructor() {}

  ngOnChanges(changes: SimpleChanges) {
    this.onViewsChange(changes);
  }

  onDeleteView(viewId: number) {
    this.delete.emit(viewId);
  }

  onAssignViewToSite(viewId: number, isAssign: boolean) {
    this.assignViewToSite.emit(<AssignViewToSite>{ viewId: viewId, siteId: this.siteSelected.id, isAssign: isAssign });
  }

  onSetDefaultView(viewId: number, isDefault: boolean) {
    this.setDefault.emit({ viewId, isDefault });
  }

  onSelectionChange() {
    this.viewSelect.next(this.viewSelected);
  }

  formatSites(viewSites: ViewSite[]) {
    if (viewSites) {
      return viewSites.map((x) => x.siteTitle).join(', ');
    } else {
      return '';
    }
  }

  containsCurrentSite(view: View) {
    if (view && view.viewSites && this.siteSelected) {
      return view.viewSites.some((x: ViewSite) => x.siteId === this.siteSelected.id);
    }
    return false;
  }

  isSiteDefault(view: View): boolean {
    if (view && view.viewSites && this.siteSelected) {
      return view.viewSites.some((x: ViewSite) => x.siteId === this.siteSelected.id && x.isDefault === true);
    }
    return false;
  }

  showDefineDefault() {
    return !(this.isSiteDefault(this.viewSelected) === true && this.canSetDefault === true);
  }

  showLinkWithSite() {
    return !(this.containsCurrentSite(this.viewSelected) === true && this.canAssign === true);
  }

  private onViewsChange(changes: SimpleChanges) {
    if (changes.views && this.table) {
      if (this.viewSelected && this.viewSelected.id > 0 && this.views) {
        this.viewSelected = this.views.filter((x) => x.id === this.viewSelected.id)[0];
      } else {
        this.viewSelected = {} as View;
      }
      this.onSelectionChange();
    }
  }
}
