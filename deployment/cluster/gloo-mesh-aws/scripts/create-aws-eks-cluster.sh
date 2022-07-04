#!/bin/bash
#
# CLUSTER_NAME
#
#

echo "Creating cluster $CLUSTER_NAME"

eksctl create cluster \
    --name $CLUSTER_NAME \
    --region eu-west-2 \
    --version 1.22
