import { AsyncPipe, NgIf } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { SpinnerComponent } from 'src/app/shared/bia-shared/components/spinner/spinner.component';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  selector: 'app-maintenance-teams-item',
  templateUrl:
    '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
  imports: [RouterOutlet, NgIf, AsyncPipe, SpinnerComponent],
})
export class MaintenanceTeamItemComponent
  extends CrudItemItemComponent<MaintenanceTeam>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
    protected layoutService: BiaLayoutService
  ) {
    super(injector, maintenanceTeamService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.maintenanceTeamService.crudItem$.subscribe(maintenanceTeam => {
        // TODO after creation of CRUD Team MaintenanceTeam : set the field of the item to display in the breadcrump
        if (maintenanceTeam?.title) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = maintenanceTeam.title;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );
  }
}
