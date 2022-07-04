#!/bin/bash
#
# CONTEXT_NAME
# ISTIO_VERSION
# ISTIO_REV
#
#

echo "Installing istio v$ISTIO_VERSION r$ISTIO_REV into $CONTEXT_NAME"

kubectl --context $CONTEXT_NAME create ns istio-system
kubectl --context $CONTEXT_NAME create ns istio-gateways

helm upgrade --install istio-base \
    ./istio-$ISTIO_VERSION/manifests/charts/base \
    -n istio-system \
    --set defaultRevision=$ISTIO_REV \
    --kube-context $CONTEXT_NAME

helm upgrade --install istio-$ISTIO_VERSION \
    ./istio-$ISTIO_VERSION/manifests/charts/istio-control/istio-discovery \
    -n istio-system \
    --values ./istio/istio-$CONTEXT_NAME.yaml \
    --kube-context $CONTEXT_NAME

kubectl label namespace istio-gateways istio.io/rev=$ISTIO_REV --context $CONTEXT_NAME

helm upgrade --install istio-ingressgateway \
    ./istio-$ISTIO_VERSION/manifests/charts/gateways/istio-ingress \
    -n istio-gateways \
    --values ./istio/istio-ingress.yaml \
    --kube-context $CONTEXT_NAME

helm upgrade --install istio-eastwestgateway \
    ./istio-$ISTIO_VERSION/manifests/charts/gateways/istio-ingress \
    -n istio-gateways \
    --values ./istio/istio-east-west-gateway.yaml \
    --kube-context $CONTEXT_NAME

kubectl apply -f ./istio-$ISTIO_VERSION/samples/addons/kiali.yaml --context $CONTEXT_NAME
