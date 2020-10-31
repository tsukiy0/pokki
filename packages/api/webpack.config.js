const path = require("path");
const webpack = require("webpack");

module.exports = {
  entry: path.join(__dirname, "src/index.ts"),
  output: {
    path: path.join(__dirname, "out"),
    filename: "index.js",
    libraryTarget: "commonjs",
  },
  target: "node",
  plugins: [
    new webpack.IgnorePlugin(/pg-native/),
    new webpack.IgnorePlugin(/mssql/),
    new webpack.IgnorePlugin(/mysql/),
    new webpack.IgnorePlugin(/mysql2/),
    new webpack.IgnorePlugin(/sqlite3/),
    new webpack.IgnorePlugin(/pg-query-stream/),
    new webpack.IgnorePlugin(/oracledb/),
  ],
  mode: "production",
  devtool: "eval",
  resolve: {
    extensions: [".js", ".ts"],
  },
  optimization: {
    minimize: false,
  },
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: {
          loader: "ts-loader",
          options: {
            configFile: "tsconfig.build.json",
          },
        },
      },
    ],
  },
};
