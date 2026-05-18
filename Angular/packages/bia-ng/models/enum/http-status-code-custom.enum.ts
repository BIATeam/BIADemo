/* eslint-disable @typescript-eslint/naming-convention */
export declare const enum HttpStatusCodeCustom {
  /**
   * Custom status code 0. Typically used for non-standard conditions like a failed connection.
   */
  FailedConnection = 0,

  /**
   * HTTP status code 498.
   */
  InvalidToken = 498, // Unofficial code, often used for authentication token issues

  /**
   * HTTP status code 426.
   */
  UpgradeRequired = 426,

  /**
   * HTTP status code 520.
   */
  UnknownServerError = 520, // Unofficial code, used when the server returns an unexpected error
}
