export function hasStringValue(str: string | null | undefined): boolean {
  if (str === null || str === undefined) {
    return false;
  }

  return str.length > 0 && str.trim().length > 0;
}
