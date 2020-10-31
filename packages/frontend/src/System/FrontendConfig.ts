import { BaseError, Config, ConfigKeyNotFoundError } from "@tsukiy0/tscore";
import fetch from "isomorphic-fetch";

export class RemoteConfigNotFoundError extends BaseError {}

export class FrontendConfig implements Config {
  constructor(private readonly config: Record<string, string>) {}

  static async fromRemote(): Promise<Config> {
    const res = await fetch("/config.json");

    if (res.status !== 200) {
      throw new RemoteConfigNotFoundError();
    }

    return new FrontendConfig(await res.json());
  }

  get(key: string): string {
    const value = this.config[key];

    if (!value) {
      throw new ConfigKeyNotFoundError(key);
    }

    return value;
  }
}
