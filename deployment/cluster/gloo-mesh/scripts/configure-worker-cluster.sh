#!/bin/bash
#
# ADMIN_CONTEXT_NAME
# DATA_CONTEXT_NAME
# DATA_CLUSTER_NAME
# GLOO_MESH_VERSION
#

kubectl apply --context $ADMIN_CONTEXT_NAME -f ./gloo/cluster-registration.yaml

kubectl --context $DATA_CONTEXT_NAME create ns gloo-mesh

ADMIN_CONTEXT_NAME=$ADMIN_CONTEXT_NAME \
    DATA_CONTEXT_NAME=$DATA_CONTEXT_NAME \
    DATA_CLUSTER_NAME=$DATA_CLUSTER_NAME \
    GLOO_MESH_VERSION=$GLOO_MESH_VERSION \
    ./gloo/register-cluster.sh
