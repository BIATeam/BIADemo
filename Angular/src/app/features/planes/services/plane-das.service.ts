import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import {
  BiaFieldDateFormat,
  PostParam,
  PutParam,
  SaveParam,
} from 'packages/bia-ng/models/public-api';
import { Plane, planeFieldsConfiguration } from '../model/plane';

@Injectable({
  providedIn: 'root',
})
export class PlaneDas extends AbstractDas<Plane> {
  constructor(injector: Injector) {
    super(injector, 'Planes');
  }

  /**
   * Extrait les champs qui utilisent la timezone UTC
   * D'après la configuration des champs, identifie les champs avec autoTimezone === 'UTC'
   */
  private getUtcFields(): string[] {
    return planeFieldsConfiguration.columns
      .filter(
        field =>
          field.displayFormat instanceof BiaFieldDateFormat &&
          field.displayFormat.autoTimezone === 'UTC'
      )
      .map(field => field.field as string);
  }

  /**
   * Override saveItem pour ajouter les champs UTC
   */
  override saveItem<Plane>(param: SaveParam<Plane>) {
    // Ajoute les champs UTC au paramètre
    if (!param.utcFields) {
      param.utcFields = this.getUtcFields();
    }
    return super.saveItem(param);
  }

  /**
   * Override putItem pour ajouter les champs UTC
   */
  override putItem<Plane>(param: PutParam<Plane>) {
    // Ajoute les champs UTC au paramètre
    if (!param.utcFields) {
      param.utcFields = this.getUtcFields();
    }
    return super.putItem(param);
  }

  /**
   * Override postItem pour ajouter les champs UTC
   */
  override postItem<Plane>(param: PostParam<Plane>) {
    // Ajoute les champs UTC au paramètre
    if (!param.utcFields) {
      param.utcFields = this.getUtcFields();
    }
    return super.postItem(param);
  }
}
