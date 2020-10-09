import { BaseError } from "@tsukiy0/tscore";
import { Set } from "./Set";

export class EmptySetError extends BaseError {}

export class NonEmptySet<T> extends Set<T> {
  constructor(items: readonly T[], itemEquals: (a: T, b: T) => boolean) {
    super(items, itemEquals);

    if (items.length === 0) {
      throw new EmptySetError();
    }
  }
}
