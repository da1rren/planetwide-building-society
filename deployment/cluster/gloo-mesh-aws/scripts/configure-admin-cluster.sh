#!/bin/bash
#
# ADMIN_CLUSTER_01
# GLOO_MESH_VERSION
# GLOO_TOKEN

source .env

echo "Installing gloo admin v$GLOO_MESH_VERSION into $ADMIN_CLUSTER_01"

helm repo add gloo-mesh-enterprise https://storage.googleapis.com/gloo-mesh-enterprise/gloo-mesh-enterprise
helm repo add gloo-mesh-agent https://storage.googleapis.com/gloo-mesh-enterprise/gloo-mesh-agent
helm repo update

kubectl --context $ADMIN_CLUSTER_01 create ns gloo-mesh

helm upgrade --install gloo-mesh-enterprise gloo-mesh-enterprise/gloo-mesh-enterprise \
    --namespace gloo-mesh --kube-context $ADMIN_CLUSTER_01 \
    --version=$GLOO_MESH_VERSION \
    --set glooMeshMgmtServer.ports.healthcheck=8091 \
    --set glooMeshUi.serviceType=LoadBalancer \
    --set mgmtClusterName=$ADMIN_CLUSTER_01 \
    --set licenseKey=$GLOO_TOKEN

kubectl apply -f ./gloo/workspaces.yaml --context $ADMIN_CLUSTER_01
