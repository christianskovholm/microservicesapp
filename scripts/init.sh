#!/bin/bash

$storage_account_name=$1
$container_name=$2
$access_key=$3

terraform init \
    -backend-config="storage_account_name=$storage_account_name" \
    -backend-config="container_name=$container_name" \
    -backend-config="access_key=$access_key" \
    -backend-config="key=codelab.microsoft.tfstate"