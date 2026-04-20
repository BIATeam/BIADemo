/* eslint-disable @typescript-eslint/naming-convention */
export enum FieldEditMode {
  /**
   * Field is not editable in any form (create or edit).
   * It is read-only and displayed as output only.
   */
  ReadOnly = 'readOnly',

  /**
   * Field is editable only when creating a new record.
   * It is disabled in the edit form.
   */
  InitializableOnly = 'initializableOnly',

  /**
   * Field is editable only when updating an existing record.
   * It is disabled in the create form.
   */
  UpdatableOnly = 'updatableOnly',

  /**
   * Field is editable in both create and edit forms.
   * This is the default behavior.
   */
  Editable = 'editable',
}
