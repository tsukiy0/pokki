#!/usr/bin/env bash

set -euxo pipefail

pushd frontend
yarn install --silent
amplify codegen
popd
