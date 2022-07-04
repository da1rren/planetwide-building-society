#!/bin/bash
#
# ADMIN_CLUSTER_NAME
# ADMIN_CONTEXT_NAME
# GLOO_MESH_VERSION
# GLOO_TOKEN

helm repo add gloo-mesh-enterprise https://storage.googleapis.com/gloo-mesh-enterprise/gloo-mesh-enterprise
helm repo add gloo-mesh-agent https://storage.googleapis.com/gloo-mesh-enterprise/gloo-mesh-agent
helm repo update

kubectl --context $ADMIN_CONTEXT_NAME create ns gloo-mesh

helm upgrade --install gloo-mesh-enterprise gloo-mesh-enterprise/gloo-mesh-enterprise \
    --namespace gloo-mesh --kube-context $ADMIN_CONTEXT_NAME \
    --version=$GLOO_MESH_VERSION \
    --set glooMeshMgmtServer.ports.healthcheck=8091 \
    --set glooMeshUi.serviceType=LoadBalancer \
    --set mgmtClusterName=$ADMIN_CLUSTER_NAME \
    --set licenseKey=$GLOO_TOKEN
