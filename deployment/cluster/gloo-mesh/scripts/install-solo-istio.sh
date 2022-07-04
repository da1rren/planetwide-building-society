#!/bin/bash
#
# CONTEXT_NAME
# ISTIO_VERSION
# ISTIO_REV
#
#

curl -L https://istio.io/downloadIstio | ISTIO_VERSION=$ISTIO_VERSION TARGET_ARCH=x86_64 sh -

kubectl config use-context $CONTEXT_NAME
kubectl create ns istio-system
kubectl create ns istio-gateways

helm upgrade --install istio-base \
    ./istio-$ISTIO_VERSION/manifests/charts/base \
    -n istio-system \
    --set defaultRevision=$ISTIO_REV

helm upgrade --install istio-$ISTIO_VERSION \
    ./istio-$ISTIO_VERSION/manifests/charts/istio-control/istio-discovery \
    -n istio-system \
    --values ./istio/istio.yaml

kubectl label namespace istio-gateways istio.io/rev=$ISTIO_REV

helm upgrade --install istio-ingressgateway \
    ./istio-$ISTIO_VERSION/manifests/charts/gateways/istio-ingress \
    -n istio-gateways \
    --values ./istio/istio-ingress.yaml

helm upgrade --install istio-eastwestgateway \
    ./istio-$ISTIO_VERSION/manifests/charts/gateways/istio-ingress \
    -n istio-gateways \
    --values ./istio/istio-east-west-gateway.yaml

kubectl apply -f ./istio-$ISTIO_VERSION/samples/addons/kiali.yaml

rm -rf istio-$ISTIO_VERSION/
