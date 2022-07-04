#!/bin/bash

source .env

time xargs -n2 -P 5 -I {} sh -c 'eval "$1"' - {} <<EOF
CLUSTER_NAME=$ADMIN_CLUSTER_01 ./scripts/delete-aws-cluster.sh 
CLUSTER_NAME=$WORKER_CLUSTER_01 ./scripts/delete-aws-cluster.sh
CLUSTER_NAME=$WORKER_CLUSTER_02 ./scripts/delete-aws-cluster.sh
CLUSTER_NAME=$WORKER_CLUSTER_03 ./scripts/delete-az-cluster.sh
EOF
