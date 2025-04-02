import { NgIf } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { Confirmation, ConfirmationService, PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { Table, TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { BiaDialogService } from 'src/app/core/bia-core/services/bia-dialog.service';
import { Team } from 'src/app/domains/bia-domains/team/model/team';
import { AssignViewToTeam } from '../../model/assign-view-to-team';
import { TeamView } from '../../model/team-view';
import { ViewTeam } from '../../model/view-team';

@Component({
  selector: 'bia-view-team-table',
  templateUrl: './view-team-table.component.html',
  styleUrls: ['./view-team-table.component.scss'],
  imports: [
    NgIf,
    ButtonDirective,
    Tooltip,
    TableModule,
    PrimeTemplate,
    ConfirmDialog,
    TranslateModule,
  ],
})
export class ViewTeamTableComponent implements OnChanges {
  @Input() views: TeamView[];
  @Input() teamSelected: Team;
  @Input() canDelete = false;
  @Input() canSetDefault = false;
  @Input() canUpdate = false;
  @Input() canAssign = false;

  get viewSelected(): TeamView | undefined {
    if (this.table && this.table.selection) {
      return this.table.selection as TeamView;
    }
    return undefined;
  }
  set viewSelected(value: TeamView | undefined) {
    if (this.table) {
      this.table.selection = value;
    }
  }

  @ViewChild('viewTeamTable', { static: false }) table: Table;

  @Output() assignViewToTeam = new EventEmitter<AssignViewToTeam>();
  @Output() delete = new EventEmitter<number>();
  @Output() setDefault = new EventEmitter<{
    viewId: number | undefined;
    isDefault: boolean;
  }>();
  @Output() viewSelect = new EventEmitter<TeamView | undefined>();

  constructor(
    protected biaDialogService: BiaDialogService,
    protected confirmationService: ConfirmationService
  ) {}

  ngOnChanges(changes: SimpleChanges) {
    this.onViewsChange(changes);
  }

  onDeleteView(viewId: number | undefined) {
    if (!viewId) {
      return;
    }
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation(),
      accept: () => {
        this.delete.emit(viewId);
      },
    };
    this.confirmationService.confirm(confirmation);
  }

  onAssignViewToTeam(viewId: number | undefined, isAssign: boolean) {
    if (!viewId) {
      return;
    }
    this.assignViewToTeam.emit(<AssignViewToTeam>{
      viewId: viewId,
      teamId: this.teamSelected.id,
      isAssign: isAssign,
    });
  }

  onAssignViewToTeamWithDelete(viewId: number | undefined, isAssign: boolean) {
    if (!viewId) {
      return;
    }
    const confirmation: Confirmation = {
      ...this.biaDialogService.getDeleteConfirmation('view-team-confirm'),
      accept: () => {
        this.assignViewToTeam.emit(<AssignViewToTeam>{
          viewId: viewId,
          teamId: this.teamSelected.id,
          isAssign: isAssign,
        });
      },
    };
    this.confirmationService.confirm(confirmation);
  }

  onSetDefaultView(viewId: number | undefined, isDefault: boolean) {
    this.setDefault.emit({ viewId, isDefault });
  }

  onSelectionChange() {
    this.viewSelect.next(this.viewSelected);
  }

  formatTeams(viewTeams: ViewTeam[]) {
    if (viewTeams) {
      return viewTeams.map(x => x.teamTitle).join(', ');
    } else {
      return '';
    }
  }

  containsCurrentTeam(view: TeamView | undefined) {
    if (view && view.viewTeams && this.teamSelected) {
      return view.viewTeams.some(
        (x: ViewTeam) => x.teamId === this.teamSelected.id
      );
    }
    return false;
  }

  containsOnlyCurrentTeam(view: TeamView | undefined) {
    if (view && view.viewTeams?.length > 1) return false;
    if (view && view.viewTeams && this.teamSelected) {
      return view.viewTeams.some(
        (x: ViewTeam) => x.teamId === this.teamSelected.id
      );
    }
    return false;
  }
  isTeamDefault(view: TeamView | undefined): boolean {
    if (view && view.viewTeams && this.teamSelected) {
      return view.viewTeams.some(
        (x: ViewTeam) =>
          x.teamId === this.teamSelected.id && x.isDefault === true
      );
    }
    return false;
  }

  showDefineDefault() {
    return !(
      this.isTeamDefault(this.viewSelected) === true &&
      this.canSetDefault === true
    );
  }

  showLinkWithTeam() {
    return (
      this.containsCurrentTeam(this.viewSelected) === false &&
      this.canAssign === true
    );
  }

  showUnLinkWithTeamAsDelete() {
    return (
      this.containsOnlyCurrentTeam(this.viewSelected) === true &&
      this.canAssign === true
    );
  }

  showUnlinkWithTeam() {
    return (
      this.containsOnlyCurrentTeam(this.viewSelected) === false &&
      this.containsCurrentTeam(this.viewSelected) === true &&
      this.canAssign === true
    );
  }

  protected onViewsChange(changes: SimpleChanges) {
    if (changes.views && this.table) {
      const viewSelected: TeamView | undefined = this.viewSelected;
      if (viewSelected && viewSelected.id > 0 && this.views) {
        this.viewSelected = this.views.filter(x => x.id === viewSelected.id)[0];
      } else {
        this.viewSelected = undefined;
      }
      this.onSelectionChange();
    }
  }
}
