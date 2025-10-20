export interface HttpErrorReport {
  errorCode: number;
  errorMessage: string;
}

export function isHttpErrorReport(error: any): error is HttpErrorReport {
  return (
    typeof error === 'object' &&
    error !== null &&
    typeof error.errorCode === 'number' &&
    typeof error.errorMessage === 'string'
  );
}
