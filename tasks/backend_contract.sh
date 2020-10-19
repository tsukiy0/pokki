#!/usr/bin/env bash

set -euxo pipefail

pushd backend
dotnet test --filter Category=Contract
popd
