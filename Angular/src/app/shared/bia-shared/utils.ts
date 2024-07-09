export const isEmpty = (value: any | null | undefined): boolean => {
  if (value == null) {
    // Covers both `null` and `undefined`
    return true;
  }

  if (value instanceof Date) {
    return Number.isNaN(value.getTime());
  }

  if (
    ['string', 'number'].includes(typeof value) ||
    value instanceof String ||
    value instanceof Number
  ) {
    return String(value).trim().length === 0;
  }

  if (Array.isArray(value) || typeof value === 'object') {
    return Object.keys(value).length === 0;
  }

  return false;
};

export const isObject = (object: any): boolean => {
  return (
    object != null &&
    object instanceof Date !== true &&
    typeof object === 'object'
  );
};

export const clone = <T>(object: T): T => {
  return JSON.parse(JSON.stringify(object));
};
