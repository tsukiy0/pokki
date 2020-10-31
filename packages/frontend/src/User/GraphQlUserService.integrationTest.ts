import { GetUserRequest, User, UserIdRandomizer } from "@pokki/core";
import { SystemConfig } from "@tsukiy0/tscore";
import AWSAppSyncClient, { AUTH_TYPE } from "aws-appsync";
import { GraphQlUserService } from "./GraphQlUserService";

describe("UserClient", () => {
  it("creates user", async () => {
    const config = new SystemConfig();

    const service = new GraphQlUserService(
      new AWSAppSyncClient({
        url: config.get("API_URL"),
        region: config.get("API_REGION"),
        auth: {
          type: AUTH_TYPE.API_KEY,
          apiKey: config.get("API_KEY"),
        },
        disableOffline: true,
      }),
    );

    const user = new User(UserIdRandomizer.random(), "bob");

    await service.createUser(user);

    const actual = await service.getUser(new GetUserRequest(user.id));

    expect(actual.equals(user)).toBeTruthy();
  });
});
