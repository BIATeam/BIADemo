import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CrudConfig } from '../../../feature-templates/crud-items/model/crud-config';

@Component({
  selector: 'bia-dynamic-layout',
  templateUrl: './dynamic-layout.component.html',
  styleUrls: ['./dynamic-layout.component.scss'],
})
export class DynamicLayoutComponent<TDto extends { id: number }> {
  configuration?: CrudConfig<TDto>;

  constructor(public activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    const snapshot = this.activatedRoute.snapshot;
    this.configuration = snapshot.data['configuration'];
  }
}
