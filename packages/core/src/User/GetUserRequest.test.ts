import {
  testComparable,
  testSerializer,
} from "@tsukiy0/tscore/dist/index.testTemplate";
import { GetUserRequest, GetUserRequestSerializer } from "./GetUserRequest";
import { UserIdRandomizer } from "./User";

describe("GetUserRequest", () => {
  testComparable(() => new GetUserRequest(UserIdRandomizer.random()));
  testSerializer(
    GetUserRequestSerializer,
    () => new GetUserRequest(UserIdRandomizer.random()),
  );
});
