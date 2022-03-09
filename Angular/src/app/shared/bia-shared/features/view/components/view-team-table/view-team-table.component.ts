import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { View } from '../../model/view';
import { AssignViewToTeam } from '../../model/assign-view-to-team';
import { ViewTeam } from '../../model/view-team';
import { Team } from 'src/app/domains/team/model/team';
import { Table } from 'primeng/table';

@Component({
  selector: 'bia-view-team-table',
  templateUrl: './view-team-table.component.html',
  styleUrls: ['./view-team-table.component.scss']
})
export class ViewTeamTableComponent implements OnChanges {
  @Input() views: View[];
  @Input() teamSelected: Team;
  @Input() canDelete = false;
  @Input() canSetDefault = false;
  @Input() canUpdate = false;
  @Input() canAssign = false;

  get viewSelected(): View {
    if (this.table && this.table.selection) {
      return this.table.selection as View;
    }
    return {} as View;
  }
  set viewSelected(value: View) {
    if (this.table) {
      this.table.selection = value;
    }
  }

  @ViewChild('viewTeamTable', { static: false }) table: Table;

  @Output() assignViewToTeam = new EventEmitter<AssignViewToTeam>();
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

  onAssignViewToTeam(viewId: number, isAssign: boolean) {
    this.assignViewToTeam.emit(<AssignViewToTeam>{ viewId: viewId, teamId: this.teamSelected.id, isAssign: isAssign });
  }

  onSetDefaultView(viewId: number, isDefault: boolean) {
    this.setDefault.emit({ viewId, isDefault });
  }

  onSelectionChange() {
    this.viewSelect.next(this.viewSelected);
  }

  formatTeams(viewTeams: ViewTeam[]) {
    if (viewTeams) {
      return viewTeams.map((x) => x.teamTitle).join(', ');
    } else {
      return '';
    }
  }

  containsCurrentTeam(view: View) {
    if (view && view.viewTeams && this.teamSelected) {
      return view.viewTeams.some((x: ViewTeam) => x.teamId === this.teamSelected.id);
    }
    return false;
  }

  isTeamDefault(view: View): boolean {
    if (view && view.viewTeams && this.teamSelected) {
      return view.viewTeams.some((x: ViewTeam) => x.teamId === this.teamSelected.id && x.isDefault === true);
    }
    return false;
  }

  showDefineDefault() {
    return !(this.isTeamDefault(this.viewSelected) === true && this.canSetDefault === true);
  }

  showLinkWithTeam() {
    return !(this.containsCurrentTeam(this.viewSelected) === true && this.canAssign === true);
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
