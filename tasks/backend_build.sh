#!/usr/bin/env bash

set -euxo pipefail

pushd backend
dotnet tool restore
dotnet restore
dotnet build
dotnet test --filter Category=Unit
dotnet lambda package -pl Api
popd
