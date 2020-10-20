#!/usr/bin/env bash

set -euxo pipefail

pushd deployment
yarn install --silent
yarn typecheck
yarn lint
yarn build
popd
