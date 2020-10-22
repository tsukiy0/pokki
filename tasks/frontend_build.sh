#!/usr/bin/env bash

set -euxo pipefail

pushd frontend
yarn install --silent
amplify codegen
yarn typecheck
yarn lint
yarn test
yarn build
popd
