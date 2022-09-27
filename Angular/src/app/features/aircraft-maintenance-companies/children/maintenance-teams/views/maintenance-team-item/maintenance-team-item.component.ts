import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { MaintenanceTeam } from '../../model/maintenance-team';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { MaintenanceTeamService } from '../../services/maintenance-team.service';

@Component({
  templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: ['../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss']
})
export class MaintenanceTeamItemComponent extends CrudItemItemComponent<MaintenanceTeam> implements OnInit {
  constructor(protected store: Store<AppState>,
    protected injector: Injector,
    public maintenanceTeamService: MaintenanceTeamService,
    protected layoutService: BiaClassicLayoutService,
  ) {
    super(injector, maintenanceTeamService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add
      (
        this.maintenanceTeamService.crudItem$.subscribe((maintenanceTeam) => {
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
