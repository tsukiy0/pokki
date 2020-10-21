#!/usr/bin/env bash

set -euxo pipefail

pushd backend
dotnet restore
dotnet build
dotnet test --filter Category=Unit
dotnet lambda package Api
popd
