import { Component, Injector, OnInit } from '@angular/core';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { Engine } from '../../model/engine';
import { EngineService } from '../../services/engine.service';
import { RouterOutlet } from '@angular/router';
import { NgIf, AsyncPipe } from '@angular/common';
import { BiaSharedModule } from '../../../../../../shared/bia-shared/bia-shared.module';

@Component({
    selector: 'app-engines-item',
    templateUrl: '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.html',
    styleUrls: [
        '../../../../../../shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component.scss',
    ],
    imports: [RouterOutlet, NgIf, BiaSharedModule, AsyncPipe]
})
export class EngineItemComponent
  extends CrudItemItemComponent<Engine>
  implements OnInit
{
  constructor(
    protected injector: Injector,
    public engineService: EngineService
  ) {
    super(injector, engineService);
  }
}
