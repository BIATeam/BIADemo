import { AsyncPipe } from '@angular/common';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemNewComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-new/crud-item-new.component';
import { PlaneFormComponent } from '../../components/plane-form/plane-form.component';
import { Plane } from '../../model/plane';
import { planeCRUDConfiguration } from '../../plane.constants';
import { PlaneOptionsService } from '../../services/plane-options.service';
import { PlaneService } from '../../services/plane.service';

@Component({
  selector: 'app-plane-new',
  templateUrl: './plane-new.component.html',
  imports: [PlaneFormComponent, AsyncPipe],
})
export class PlaneNewComponent
  extends CrudItemNewComponent<Plane>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    protected planeOptionsService: PlaneOptionsService,
    public planeService: PlaneService
  ) {
    super(injector, planeService);
    this.crudConfiguration = planeCRUDConfiguration;
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.sub.add(
      this.biaTranslationService.currentCulture$.subscribe(() => {
        this.planeOptionsService.loadAllOptions();
      })
    );
  }
}
