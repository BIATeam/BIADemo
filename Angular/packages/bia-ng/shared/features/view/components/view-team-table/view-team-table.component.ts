import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { BiaDialogService } from 'packages/bia-ng/core/public-api';
import { CurrentTeamDto, Team } from 'packages/bia-ng/models/public-api';
import { Confirmation, ConfirmationService, PrimeTemplate } from 'primeng/api';
import { ButtonDirective } from 'primeng/button';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { Table, TableModule } from 'primeng/table';
import { Tooltip } from 'primeng/tooltip';
import { AssignViewToTeam } from '../../model/assign-view-to-team';
import { View } from '../../model/view';
import { ViewTeam } from '../../model/view-team';

@Component({
  selector: 'bia-view-team-table',
  templateUrl: './view-team-table.component.html',
  styleUrls: ['./view-team-table.component.scss'],
  imports: [
    ButtonDirective,
    Tooltip,
    TableModule,
    PrimeTemplate,
    ConfirmDialog,
    TranslateModule,
  ],
})
export class ViewTeamTableComponent {
  @Input() views: View[];
  @Input() originTeam: CurrentTeamDto | undefined;
  @Input() teamSelected: Team;
  @Input() canDelete = false;
  @Input() canSetDefault = false;
  @Input() canUpdate = false;
  @Input() canAssign = false;

  @ViewChild('viewTeamTable', { static: false }) table: Table;

  @Output() assignViewToTeam = new EventEmitter<AssignViewToTeam>();
  @Output() setDefault = new EventEmitter<{
    viewId: number | undefined;
    isDefault: boolean;
  }>();
  @Output() viewSelect = new EventEmitter<View | undefined>();

  get unlinkWithTeamTooltipLabel(): string {
    return `${this.translateService.instant(
      'bia.views.unlinkWithTeam'
    )} ${this.originTeam?.teamTitle}`;
  }

  get linkWithTeamTooltipLabel(): string {
    return `${this.translateService.instant(
      'bia.views.linkWithTeam'
    )} ${this.originTeam?.teamTitle}`;
  }

  constructor(
    protected biaDialogService: BiaDialogService,
    protected confirmationService: ConfirmationService,
    private translateService: TranslateService
  ) {}

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

  toggleTeamDefaultView(view: View) {
    this.setDefault.emit({
      viewId: view.id,
      isDefault: !this.isTeamDefault(view),
    });
  }

  formatTeams(viewTeams: ViewTeam[]) {
    if (viewTeams) {
      return viewTeams.map(x => x.teamTitle).join(', ');
    } else {
      return '';
    }
  }

  containsCurrentTeam(view: View | undefined) {
    if (view && view.viewTeams && this.teamSelected) {
      return view.viewTeams.some(
        (x: ViewTeam) => x.id === this.teamSelected.id
      );
    }
    return false;
  }

  containsOnlyCurrentTeam(view: View | undefined) {
    if (view && view.viewTeams?.length > 1) return false;
    if (view && view.viewTeams && this.teamSelected) {
      return view.viewTeams.some(
        (x: ViewTeam) => x.id === this.teamSelected.id
      );
    }
    return false;
  }

  isTeamDefault(view: View): boolean {
    if (view.viewTeams && this.teamSelected) {
      return view.viewTeams.some(
        (x: ViewTeam) => x.id === this.teamSelected.id && x.isDefault === true
      );
    }
    return false;
  }

  showLinkWithTeam(view: View) {
    return this.containsCurrentTeam(view) === false && this.canAssign === true;
  }

  showUnLinkWithTeamAsDelete(view: View) {
    return (
      this.containsOnlyCurrentTeam(view) === true && this.canAssign === true
    );
  }

  showUnlinkWithTeam(view: View) {
    return (
      this.containsOnlyCurrentTeam(view) === false &&
      this.containsCurrentTeam(view) === true &&
      this.canAssign === true
    );
  }
}
