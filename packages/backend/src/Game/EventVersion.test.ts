import { testComparable } from "@tsukiy0/tscore/dist/index.testTemplate";
import { EventVersion } from "./EventVersion";

describe("EventVersion", () => {
  testComparable(() => new EventVersion(1));
});
