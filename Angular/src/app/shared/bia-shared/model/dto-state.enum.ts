export enum DtoState {
  /**
   * The DTO is unchanged.
   */
  Unchanged = 0,
  /**
   * The DTO is new and marked to be added to the context.
   */
  Added = 1,
  /**
   * The DTO has been modified.
   */
  Modified = 2,
  /**
   * The DTO is marked to be deleted in the context.
   */
  Deleted = 3
}
