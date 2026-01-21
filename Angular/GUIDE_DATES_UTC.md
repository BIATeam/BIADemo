/\*\*

- GUIDE: Gestion des Dates UTC dans BIA Framework
-
- Cette solution permet de gérer correctement les dates UTC picker (dates affichées en UTC)
- lors de l'envoi au backend, en évitant les offset de timezone indésirés.
  \*/

// ============================================================================
// 1. CONCEPT
// ============================================================================
//
// Le framework gère deux types de dates :
//
// MODE NORMAL:
// - Les dates sont affichées et manipulées selon la timezone locale
// - Ex: Un utilisateur en timezone CET voit "15:30" pour "14:30 UTC"
// - Sérialisation: toISOString() standard
//
// MODE UTC (autoTimezone === 'UTC'):
// - Les dates sont affichées et manipulées en UTC
// - Ex: Un datepicker affiche "14:30" pour "2024-01-15T14:30:00Z"
// - Sérialisation spéciale: toISOStringFromUtcPickerDate()
//
// ============================================================================
// 2. IMPLEMENTATION CÔTÉ FRONTEND
// ============================================================================
//
// Dans BiaTableInputComponent (ngOnInit):
//
// if (this.field.displayFormat instanceof BiaFieldDateFormat &&
// this.field.displayFormat.autoTimezone === 'UTC') {
// const value = this.form.controls[this.field.field].value;
// if (value instanceof Date) {
// const utcDate = DateHelperService.toUtcPickerDate(value);
// this.form.controls[this.field.field].setValue(utcDate, {
// emitEvent: false,
// });
// }
// }
//
// ============================================================================
// 3. IMPLEMENTATION CÔTÉ SERVICE CRUD
// ============================================================================
//
// Quand vous appellez le service pour sauvegarder, vous devez transmettre
// la liste des champs UTC pour que la sérialisation soit correcte:
//
// AVANT (ne fonctionne pas pour UTC):
//
// const crudItem = this.form.value;
// this.crudItemService.update(crudItem).subscribe(...);
//
// APRÈS (fonctionne correctement):
//
// const crudItem = this.form.value;
// const utcFields = ['startDate', 'endDate']; // Les champs UTC
// this.crudItemService.update(crudItem, { utcFields }).subscribe(...);
//
// ============================================================================
// 4. IMPLEMENTATION CÔTÉ CRUDITEMSERVICE
// ============================================================================
//
// Dans votre CrudItemService (qui étend une classe avec DAS), vous pouvez:
//
// A) Extraire les champs UTC automatiquement depuis la config:
//
// update(item: CrudItem, options?: any) {
// const crudConfig = this.crudConfig; // Votre configuration
// const utcFields = crudConfig.fieldsConfig.columns
// .filter(f => f.displayFormat instanceof BiaFieldDateFormat &&
// f.displayFormat.autoTimezone === 'UTC')
// .map(f => f.field as string);
//
// return this.putItem({
// item,
// id: item.id,
// utcFields
// });
// }
//
// B) Ou le passer explicitement:
//
// update(item: CrudItem, options?: { utcFields?: string[] }) {
// return this.putItem({
// item,
// id: item.id,
// utcFields: options?.utcFields
// });
// }
//
// ============================================================================
// 5. METHODES DISPONIBLES DANS DateHelperService
// ============================================================================
//
// toUtcPickerDate(d: Date): Date
// - Convertit une date UTC en date locale qui affiche visuellement l'heure UTC
// - À utiliser à la réception des données du backend
// - Exemple: "2024-01-15T14:30:00Z" → Date(2024,0,15,14,30) [locale]
//
// toISOStringFromUtcPickerDate(d: Date): string
// - Convertit une date UTC picker vers son ISO string correct
// - À utiliser lors de la sérialisation pour l'envoi au backend
// - Exemple: Date(2024,0,15,14,30) [locale] → "2024-01-15T14:30:00Z"
//
// fillDateWithUtcFields(data: T, utcFields: string[]): void
// - Sérialise les dates d'un objet en tenant compte des champs UTC
// - Appelée automatiquement par GenericDasService.saveItem/putItem/postItem
// - Si utcFields est fourni, utilise toISOStringFromUtcPickerDate pour ces champs
//
// ============================================================================
// 6. EXEMPLE COMPLET
// ============================================================================
//
// DTO:
//
// export class FlightDto {
// id: number;
// name: string;
// departureTime: Date; // UTC picker
// arrivalTime: Date; // UTC picker
// createdAt: Date; // Normal (timezone locale)
// }
//
// Configuration:
//
// const flightFields = [
// new BiaFieldConfig('id', 'ID'),
// new BiaFieldConfig('name', 'Name'),
// new BiaFieldConfig('departureTime', 'Departure Time').displayFormat =
// new BiaFieldDateFormat() { autoTimezone: 'UTC' },
// new BiaFieldConfig('arrivalTime', 'Arrival Time').displayFormat =
// new BiaFieldDateFormat() { autoTimezone: 'UTC' },
// new BiaFieldConfig('createdAt', 'Created At').displayFormat =
// new BiaFieldDateFormat() { autoTimezone: '' } // Normal
// ];
//
// Service:
//
// export class FlightCrudService extends CrudItemService {
//  
// update(flight: FlightDto) {
// // Extraire les champs UTC automatiquement
// const utcFields = this.getUtcFields();
//  
// return this.putItem({
// item: flight,
// id: flight.id,
// utcFields // ['departureTime', 'arrivalTime']
// });
// }
//
// private getUtcFields(): string[] {
// return this.crudConfig.fieldsConfig.columns
// .filter(f => f.displayFormat instanceof BiaFieldDateFormat &&
// f.displayFormat.autoTimezone === 'UTC')
// .map(f => f.field as string);
// }
// }
//
// ============================================================================
// 7. FLUX DE DONNÉES
// ============================================================================
//
// Données reçues du backend:
// "2024-01-15T14:30:00Z"
// ↓
// DateHelperService.fillDate() [réception]
// ↓
// Date JavaScript en UTC: Date(2024,0,15,...) [utc)
// ↓
// DateHelperService.toUtcPickerDate() [affichage]
// ↓
// Date locale affichant 14:30: Date(2024,0,15,14,30,...) [local]
// ↓
// Utilisateur voit "14:30" dans le datepicker ✓
// ↓
// Utilisateur modifie la date
// ↓
// Form value: Date(2024,0,15,14,30,...) [local]
// ↓
// DateHelperService.fillDateWithUtcFields(['departureTime'])
// ↓
// Utilise toISOStringFromUtcPickerDate()
// ↓
// Envoie au backend: "2024-01-15T14:30:00Z" ✓
// ↓
// Backend reçoit la valeur UTC correcte ✓
//
// ============================================================================
