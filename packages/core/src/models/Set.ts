import { BaseError, Comparable, isArrayEqual } from "@tsukiy0/tscore";

export class DuplicateSetItemError extends BaseError {}

export class Set<T> implements Comparable {
  constructor(
    public readonly items: readonly T[],
    private readonly itemEquals: (a: T, b: T) => boolean,
  ) {
    const duplicates = items.filter((item, index) => {
      const duplicateIndex = items.findIndex((_) => this.itemEquals(item, _));
      return duplicateIndex !== index;
    });

    if (duplicates.length !== 0) {
      throw new DuplicateSetItemError();
    }
  }

  public readonly equals = (input: this): boolean => {
    return isArrayEqual(this.items, input.items, this.itemEquals);
  };
}
