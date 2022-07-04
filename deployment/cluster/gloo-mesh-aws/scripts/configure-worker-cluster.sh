#!/bin/bash
#
# ADMIN_CONTEXT_NAME - The admin cluster kubectl context name with gloo mesh instealled
# WORKER_CONTEXT_NAME - The cluster to install the gloo mesh agent on
# GLOO_MESH_VERSION - The version of gloo mesh to install
#

kubectl apply --context $ADMIN_CONTEXT_NAME -f gloo/cluster-$WORKER_CONTEXT_NAME.yaml

kubectl --context $WORKER_CONTEXT_NAME create ns gloo-mesh

echo "Polling $WORKER_CONTEXT_NAME ingress until external ip is available"
until kubectl get service/gloo-mesh-mgmt-server -n gloo-mesh --context $ADMIN_CONTEXT_NAME --output=jsonpath='{.status.loadBalancer}' | grep "ingress"; do :; done

LB_IP=$(kubectl get svc -n gloo-mesh gloo-mesh-mgmt-server --context $ADMIN_CONTEXT_NAME -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')
LB_PORT=$(kubectl -n gloo-mesh get service gloo-mesh-mgmt-server --context $ADMIN_CONTEXT_NAME -o jsonpath='{.spec.ports[?(@.name=="grpc")].port}')

meshctl cluster register \
    --kubecontext $ADMIN_CONTEXT_NAME \
    --remote-context $WORKER_CONTEXT_NAME \
    --relay-server-address $LB_IP:$LB_PORT \
    --gloo-mesh-agent-chart-values=gloo-mesh-config.yaml \
    --version $GLOO_MESH_VERSION \
    $WORKER_CONTEXT_NAME

kubectl apply --context $WORKER_CONTEXT_NAME -f gloo/gateway-$WORKER_CONTEXT_NAME.yaml
