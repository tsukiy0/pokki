#!/usr/bin/env bash

set -euo pipefail

get_cfn_output() {
    echo $(aws cloudformation describe-stacks --stack-name ${CFN_STACK_NAME} --query "Stacks[0].Outputs[?OutputKey==\`${1}\`].OutputValue" --output text)
}

export API_KEY=$(get_cfn_output ApiKey)
export API_URL=$(get_cfn_output ApiUrl)
export API_REGION=$(get_cfn_output ApiRegion)

pushd frontend
yarn test:contract
popd