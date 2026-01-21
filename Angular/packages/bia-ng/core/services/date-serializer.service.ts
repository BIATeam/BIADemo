import { Injectable } from '@angular/core';
import {
  BiaFieldConfig,
  BiaFieldDateFormat,
} from 'packages/bia-ng/models/public-api';
import { DateHelperService } from './date-helper.service';

/**
 * Service pour sérialiser les dates en tenant compte de la configuration UTC
 * Utilisé notamment lors de l'envoi des données au backend
 */
@Injectable({
  providedIn: 'root',
})
export class DateSerializerService {
  /**
   * Sérialise les dates en fonction de la configuration des champs.
   * Pour les champs avec autoTimezone === 'UTC', utilise la conversion UTC picker.
   *
   * @param data L'objet à sérialiser
   * @param fieldsConfig Configuration des champs (optionnel mais recommandé)
   * @returns L'objet avec dates sérialisées en ISO
   */
  public static serializeWithFieldConfig<TOut>(
    data: TOut,
    fieldsConfig?: BiaFieldConfig<TOut>[]
  ): TOut {
    if (!data) return data;

    const utcFields = new Set<string>();

    // Construit la liste des champs UTC à partir de la configuration
    if (fieldsConfig) {
      fieldsConfig.forEach(field => {
        if (
          field.displayFormat instanceof BiaFieldDateFormat &&
          field.displayFormat.autoTimezone === 'UTC'
        ) {
          utcFields.add(field.field as string);
        }
      });
    }

    DateSerializerService.serializeDates(data, utcFields);
    return data;
  }

  /**
   * Sérialise les dates de manière récursive.
   * Les champs en UTC utilisent toISOStringFromUtcPickerDate
   * Les autres utilisent toISOString standard
   */
  private static serializeDates(data: any, utcFields: Set<string>): void {
    if (!data || typeof data !== 'object') {
      return;
    }

    Object.keys(data).forEach((key: string) => {
      const value = data[key];

      if (value instanceof Date) {
        // Vérifie si c'est un champ UTC
        if (utcFields.has(key)) {
          data[key] = DateHelperService.toISOStringFromUtcPickerDate(value);
        } else {
          data[key] = value.toISOString();
        }
      } else if (value instanceof Object) {
        // Traite les objets imbriqués (pour les champs imbriqués, pas de gestion UTC spéciale)
        DateSerializerService.serializeDates(value, utcFields);
      }
    });
  }
}
