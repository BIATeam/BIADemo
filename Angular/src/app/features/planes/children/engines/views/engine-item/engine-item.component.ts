import { Component, Injector, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { first } from 'rxjs/operators';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { AppState } from 'src/app/store/state';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';

@Component({
  selector: 'app-engines-item',
  templateUrl:
    '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: [
    '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
  ],
})
export class EngineItemComponent
  extends CrudItemItemComponent<Engine>
  implements OnInit
{
  constructor(
    protected store: Store<AppState>,
    protected injector: Injector,
    public engineService: EngineService,
    protected layoutService: BiaLayoutService
  ) {
    super(injector, engineService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add(
      this.engineService.crudItem$.subscribe(engine => {
        // TODO after creation of CRUD Engine : set the field of the item to display in the breadcrump
        if (engine?.reference) {
          this.route.data.pipe(first()).subscribe(routeData => {
            (routeData as any)['breadcrumb'] = engine.reference;
          });
          this.layoutService.refreshBreadcrumb();
        }
      })
    );
  }
}
