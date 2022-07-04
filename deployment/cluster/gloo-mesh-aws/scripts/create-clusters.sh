#!/bin/bash

source .env

time xargs -n2 -P 5 -I {} sh -c 'eval "$1"' - {} <<EOF
CLUSTER_NAME=$ADMIN_CLUSTER_01 ./scripts/create-aws-eks-cluster.sh 
CLUSTER_NAME=$WORKER_CLUSTER_01 ./scripts/create-aws-eks-cluster.sh
CLUSTER_NAME=$WORKER_CLUSTER_02 ./scripts/create-aws-eks-cluster.sh
EOF

aws eks update-kubeconfig --region eu-west-2 --name $ADMIN_CLUSTER_01 --alias $ADMIN_CLUSTER_01
aws eks update-kubeconfig --region eu-west-2 --name $WORKER_CLUSTER_01 --alias $WORKER_CLUSTER_01
aws eks update-kubeconfig --region eu-west-2 --name $WORKER_CLUSTER_02 --alias $WORKER_CLUSTER_02
