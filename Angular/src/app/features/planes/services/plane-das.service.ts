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
   * Extrait les champs qui utilisent le mode LOCAL TIME (DateTimeOffset backend)
   * D'après la configuration des champs, identifie les champs avec autoTimezone === '' (vide)
   * Par défaut, tous les autres champs sont en mode UTC (DateTime backend)
   */
  private getLocalTimeFields(): string[] {
    return planeFieldsConfiguration.columns
      .filter(
        field =>
          field.displayFormat instanceof BiaFieldDateFormat &&
          field.displayFormat.autoTimezone === ''
      )
      .map(field => field.field as string);
  }

  /**
   * Override saveItem pour ajouter les champs LOCAL TIME
   */
  override saveItem<Plane>(param: SaveParam<Plane>) {
    // Ajoute les champs LOCAL TIME au paramètre
    if (!param.localTimeFields) {
      param.localTimeFields = this.getLocalTimeFields();
    }
    return super.saveItem(param);
  }

  /**
   * Override putItem pour ajouter les champs LOCAL TIME
   */
  override putItem<Plane>(param: PutParam<Plane>) {
    // Ajoute les champs LOCAL TIME au paramètre
    if (!param.localTimeFields) {
      param.localTimeFields = this.getLocalTimeFields();
    }
    return super.putItem(param);
  }

  /**
   * Override postItem pour ajouter les champs LOCAL TIME
   */
  override postItem<Plane>(param: PostParam<Plane>) {
    // Ajoute les champs LOCAL TIME au paramètre
    if (!param.localTimeFields) {
      param.localTimeFields = this.getLocalTimeFields();
    }
    return super.postItem(param);
  }
}
