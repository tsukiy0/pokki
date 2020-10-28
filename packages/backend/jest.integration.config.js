const base = require("./jest.config");

module.exports = {
  ...base,
  testMatch: ["**/*.integrationTest.ts"],
  setupFiles: ["./src/setupTests.integration.ts"],
};
