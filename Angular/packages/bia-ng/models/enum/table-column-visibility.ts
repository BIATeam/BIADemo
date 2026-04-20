/* eslint-disable @typescript-eslint/naming-convention */
export enum TableColumnVisibility {
  /**
   * Column is not available in the table at all.
   * Not shown in the column selector and never displayed.
   * Use for fields that should only appear in forms (e.g., rowVersion).
   */
  Hidden = 'hidden',

  /**
   * Column is available in the column selector but hidden by default.
   * User can manually enable it from the column selector.
   * Use for optional/advanced fields that most users don't need.
   */
  AvailableButHidden = 'availableButHidden',

  /**
   * Column is available in the column selector and visible by default.
   * This is the standard visibility for most table columns.
   */
  Visible = 'visible',
}
