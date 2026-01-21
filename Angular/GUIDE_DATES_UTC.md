# GUIDE: Gestion des Dates dans BIA Framework

Cette solution permet de gérer deux modes de dates :

- **Mode par défaut UTC** (DateTime backend) - Comportement historique
- **Mode nouveau Local Time** (DateTimeOffset backend) - Nouveau cas d'usage

## 1. CONCEPT - DEUX MODES DE FONCTIONNEMENT

Le framework supporte deux modes pour gérer les dates selon le type backend :

### MODE PAR DÉFAUT UTC (comportement historique)

- **Backend** : DateTime (UTC stocké)
- **Frontend** : Conversion `toUtc()` avant sérialisation ISO
- **Affichage** : Date en UTC pour tous les utilisateurs (avec `toUtcPickerDate()` si configuré)
- **Exemple** : Un vol à "14:30 UTC" s'affiche "14:30" partout
- **Sérialisation** : `toUtc()` puis `toISOString()`
- **Compatibilité** : **Par défaut** - tout le code existant fonctionne tel quel ✅
- **Configuration** : `autoTimezone === 'UTC'` OU non défini

### MODE NOUVEAU LOCAL TIME (`autoTimezone === ''` vide)

- **Backend** : DateTimeOffset (avec offset de timezone)
- **Frontend** : Sérialisation ISO standard (préserve offset)
- **Affichage** : Date adaptée au fuseau horaire du client
- **Exemple** : Un événement à "15:30 CET" s'affiche "09:30" pour un client à NY
- **Sérialisation** : `toISOString()` standard
- **Backend reçoit** : `"2024-01-15T15:30:00+01:00"`
- **Configuration** : `autoTimezone === ''` (chaîne vide explicite)

## 2. CONFIGURATION DES CHAMPS

### MODE PAR DÉFAUT UTC (DateTime backend) - Comportement historique

```typescript
// Option 1: Avec autoTimezone explicite 'UTC'
Object.assign(new BiaFieldConfig('firstFlightDate', 'plane.firstFlightDate'), {
  type: PropType.DateTime,
  displayFormat: Object.assign(new BiaFieldDateFormat(), {
    autoTimezone: 'UTC', // ← Mode UTC explicite
  }),
});

// Option 2: Sans displayFormat (par défaut = UTC)
Object.assign(new BiaFieldConfig('createdDate', 'plane.createdDate'), {
  type: PropType.DateTime,
  // Pas de displayFormat = mode UTC par défaut
});
```

### MODE NOUVEAU LOCAL TIME (DateTimeOffset backend)

```typescript
Object.assign(new BiaFieldConfig('eventDate', 'event.eventDate'), {
  type: PropType.DateTime,
  displayFormat: Object.assign(new BiaFieldDateFormat(), {
    autoTimezone: '', // ← Chaîne VIDE = Mode local time
  }),
});
```

## 3. BACKEND - TYPES DE DONNÉES

### MODE PAR DÉFAUT UTC

```csharp
public class PlaneDto
{
    public DateTime FirstFlightDate { get; set; }  // ← DateTime
    // Stocké en UTC dans la base
    // Le frontend envoie : "2024-01-15T14:30:00Z"
    // Le backend reçoit et stocke : DateTime en UTC
}
```

### MODE NOUVEAU LOCAL TIME

```csharp
public class EventDto
{
    public DateTimeOffset EventDate { get; set; }  // ← DateTimeOffset
    // Préserve l'offset de timezone
    // Le frontend envoie : "2024-01-15T15:30:00+01:00"
    // Le backend reçoit : DateTimeOffset avec offset +01:00
    // .UtcDateTime donne l'heure UTC équivalente
}
```

## 4. IMPLEMENTATION CÔTÉ SERVICE (DAS)

Dans votre service DAS, extrayez automatiquement les champs en mode local time depuis la configuration :

```typescript
import { Injectable, Injector } from '@angular/core';
import { AbstractDas } from 'packages/bia-ng/core/public-api';
import {
  BiaFieldDateFormat,
  SaveParam,
  PutParam,
  PostParam,
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
   * Extrait les champs en mode LOCAL TIME (autoTimezone === '')
   * Ces champs sont des exceptions au comportement par défaut UTC
   */
  private getLocalTimeFields(): string[] {
    return planeFieldsConfiguration.columns
      .filter(
        field =>
          field.displayFormat instanceof BiaFieldDateFormat &&
          field.displayFormat.autoTimezone === '' // ← Chaîne vide = local time
      )
      .map(field => field.field as string);
  }

  override saveItem<TOut>(param: SaveParam<Plane>) {
    if (!param.localTimeFields) {
      param.localTimeFields = this.getLocalTimeFields();
    }
    return super.saveItem(param);
  }

  override putItem<TOut>(param: PutParam<Plane>) {
    if (!param.localTimeFields) {
      param.localTimeFields = this.getLocalTimeFields();
    }
    return super.putItem(param);
  }

  override postItem<TOut>(param: PostParam<Plane>) {
    if (!param.localTimeFields) {
      param.localTimeFields = this.getLocalTimeFields();
    }
    return super.postItem(param);
  }
}
```

## 5. FLUX DE DONNÉES COMPLETS

### MODE HISTORIQUE UTC (autoTimezone === 'UTC')

**Réception depuis backend:**

```
Backend DateTime (UTC): 2024-01-15 14:30:00
   ↓
Envoyé en JSON: "2024-01-15T14:30:00Z"
   ↓
fillDate() convertit en Date JS
   ↓
toUtcPickerDate() pour affichage
   ↓
Datepicker affiche: "14:30" (visuel UTC)
   ↓
Utilisateur modifie: "15:30"
   ↓
fillDateWithUtcFields() avec utcFields=['firstFlightDate']
   ↓
toUtc() convertit: 15:30 local → 14:30 UTC
   ↓
toISOString(): "2024-01-15T14:30:00Z"
   ↓
Backend DateTime reçoit: 14:30 UTC ✓
```

### MODE NOUVEAU LOCAL TIME (autoTimezone === '')

**Réception depuis backend:**

```
Backend DateTimeOffset: 2024-01-15 15:30:00 +01:00
   ↓
Envoyé en JSON: "2024-01-15T15:30:00+01:00"
   ↓
fillDate() convertit en Date JS (adapté timezone client)
   ↓
Datepicker affiche selon timezone client
   Client CET: "15:30"
   Client NY: "09:30"
   ↓
Utilisateur modifie selon sa timezone
   ↓
fillDateWithUtcFields() SANS ce champ dans utcFields
   ↓
toISOString() standard avec offset client
   ↓
Envoi: "2024-01-15T16:30:00+01:00" (si modifié à 16:30 CET)
   ↓
Backend DateTimeOffset reçoit avec offset ✓
```

## 6. AVANTAGES DE CETTE APPROCHE

### ✅ Compatibilité Rétroactive

- Le code existant continue de fonctionner sans modification
- Les champs DateTime historiques utilisent automatiquement le mode UTC
- Pas de refactorisation massive nécessaire

### ✅ Flexibilité

- Choisissez le mode approprié champ par champ
- DateTime backend pour données UTC fixes (vols, logs, etc.)
- DateTimeOffset backend pour événements localisés (réunions, rendez-vous, etc.)

### ✅ Clarté

- Par défaut = mode UTC (comportement historique préservé)
- `autoTimezone === ''` = Mode local time explicite (nouveau cas d'usage)
- La configuration reflète le type backend

### ✅ Automatisation

- Les champs local time sont extraits automatiquement de la config
- Pas besoin de maintenir des listes manuellement
- Un seul endroit pour configurer : le BiaFieldConfig

## 7. QUAND UTILISER QUEL MODE ?

### Utilisez MODE PAR DÉFAUT UTC quand :

- ✅ Vous avez des données existantes en DateTime (backend)
- ✅ La date représente un moment absolu (vol, log, événement global)
- ✅ Tous les utilisateurs doivent voir la même heure
- ✅ Exemple : Heure de décollage d'un avion
- ✅ **C'est le comportement par défaut - aucune configuration spéciale requise**

### Utilisez MODE NOUVEAU LOCAL TIME quand :

- ✅ Vous créez un nouveau champ avec DateTimeOffset backend
- ✅ La date représente un événement local (réunion, rendez-vous)
- ✅ Les utilisateurs doivent voir l'heure adaptée à leur timezone
- ✅ Vous pouvez utiliser DateTimeOffset côté backend
- ✅ Exemple : Réunion d'équipe à 15h à Paris
- ✅ **Configuration explicite avec `autoTimezone === ''` requise**

## 8. MIGRATION PROGRESSIVE

Vous pouvez migrer progressivement :

1. **Gardez l'existant** : Tous les champs DateTime restent en mode UTC (par défaut)
2. **Ajoutez nouveaux champs** : Utilisez DateTimeOffset + mode local time (opt-in)
3. **Migrez sélectivement** : Changez champ par champ selon les besoins

Aucune "big bang" migration requise ! 🎉

**Important** : Le comportement par défaut préserve la compatibilité totale avec l'existant.
