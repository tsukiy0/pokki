import { Handler } from "@pokki/backend";
import { Void, VoidSerializer } from "./Void";

export class HealthCheckHandler extends Handler<Void, Void> {
  constructor() {
    super(VoidSerializer, VoidSerializer);
  }

  async handle(): Promise<Void> {
    return new Void();
  }
}
