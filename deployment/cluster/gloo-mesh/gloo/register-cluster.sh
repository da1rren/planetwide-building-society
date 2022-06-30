#!/bin/bash

## Depends on:
# ADMIN_CONTEXT_NAME - The admin cluster kubectl context name with gloo mesh instealled
# DATA_CONTEXT_NAME - The cluster to install the gloo mesh agent on
# DATA_CLUSTER_NAME - The name to give the cluster with the agent on
# GLOO_MESH_VERSION - The version of gloo mesh to install
echo "Polling ingress until external ip is available"
until kubectl get service/gloo-mesh-mgmt-server -n gloo-mesh --context $ADMIN_CONTEXT_NAME --output=jsonpath='{.status.loadBalancer}' | grep "ingress"; do :; done

LB_IP=$(kubectl get svc -n gloo-mesh gloo-mesh-mgmt-server --context $ADMIN_CONTEXT_NAME -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
LB_PORT=$(kubectl -n gloo-mesh get service gloo-mesh-mgmt-server --context $ADMIN_CONTEXT_NAME -o jsonpath='{.spec.ports[?(@.name=="grpc")].port}')
echo "$LB_IP:$LB_PORT"

meshctl cluster register \
    --kubecontext $ADMIN_CONTEXT_NAME \
    --remote-context $DATA_CONTEXT_NAME \
    --relay-server-address $LB_IP:$LB_PORT \
    --gloo-mesh-agent-chart-values=gloo-mesh-config.yaml \
    --version $GLOO_MESH_VERSION \
    $DATA_CLUSTER_NAME
