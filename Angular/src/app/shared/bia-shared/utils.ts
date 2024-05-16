export const isEmpty = (value: any): boolean => {
  if (value == null) {
    return true;
  }

  if (
    typeof value === 'string' ||
    value instanceof String ||
    typeof value === 'number' ||
    value instanceof Number
  ) {
    return String(value).trim().length === 0;
  }

  if (Array.isArray(value)) {
    return value.length === 0;
  }

  if (typeof value === 'object') {
    return Object.keys(value).length === 0;
  }

  // TODO Date?
  return false;
};

export const isObject = (object: any): boolean => {
  return object != null && object instanceof Date !== true && typeof object === 'object';
}

export const clone = (object: any): any => {
  return JSON.parse(JSON.stringify(object));
}
