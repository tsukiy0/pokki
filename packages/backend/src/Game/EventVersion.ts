import { BaseError, Comparable } from "@tsukiy0/tscore";

export class BadEventVersionNumberError extends BaseError {
  constructor(public readonly value: number) {
    super();
  }
}

export class EventVersion implements Comparable {
  constructor(private readonly value: number) {
    if (value <= 0) {
      throw new BadEventVersionNumberError(value);
    }
  }

  toNumber(): number {
    return this.value;
  }

  equals(input: this): boolean {
    return this.value === input.value;
  }
}
