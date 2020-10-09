import {
  BaseError,
  Comparable,
  isArrayEqual,
  Serializer,
} from "@tsukiy0/tscore";

export class EmptySetError extends BaseError {}
export class DuplicateSetItemError extends BaseError {}

export class NonEmptySet<T> implements Comparable {
  constructor(
    public readonly items: readonly T[],
    private readonly itemEquals: (a: T, b: T) => boolean,
  ) {
    if (items.length === 0) {
      throw new EmptySetError();
    }

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

export type NonEmptySetJson<TJson> = {
  items: readonly TJson[];
};

export class NonEmptySetSerializer<U extends NonEmptySet<T>, T, TJson>
  implements Serializer<U, NonEmptySetJson<TJson>> {
  constructor(
    private readonly itemSerializer: Serializer<T, TJson>,
    private readonly toU: (input: readonly T[]) => U,
  ) {}

  public readonly serialize = (input: U): NonEmptySetJson<TJson> => {
    return {
      items: input.items.map(this.itemSerializer.serialize),
    };
  };

  public readonly deserialize = (input: NonEmptySetJson<TJson>): U => {
    return this.toU(input.items.map(this.itemSerializer.deserialize));
  };
}
