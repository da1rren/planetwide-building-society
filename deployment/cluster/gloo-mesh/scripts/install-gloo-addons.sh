#!/bin/bash
#
# DATA_CONTEXT_NAME
# GLOO_MESH_VERSION
#

kubectl --context $DATA_CONTEXT_NAME create namespace gloo-mesh-addons
kubectl --context $DATA_CONTEXT_NAME label namespace gloo-mesh-addons istio.io/rev=1-13

helm upgrade --install gloo-mesh-agent-addons gloo-mesh-agent/gloo-mesh-agent \
    --namespace gloo-mesh-addons \
    --kube-context=$DATA_CONTEXT_NAME \
    --set glooMeshAgent.enabled=false \
    --set rate-limiter.enabled=true \
    --set ext-auth-service.enabled=true \
    --version $GLOO_MESH_VERSION
