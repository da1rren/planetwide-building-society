#!/bin/bash

source .env

WORKER_CLUSTER=planetary-core-mesh-01

kubectl create namespace planetwide --context $WORKER_CLUSTER
kubectl create -f istio-peer-security-policy.yaml --context $WORKER_CLUSTER
kubectl label namespace planetwide istio.io/rev=$ISTIO_REV --context $WORKER_CLUSTER --overwrite
helm upgrade --install --kube-context $WORKER_CLUSTER planetwide ../../charts/planetwide --namespace planetwide
kubectl apply --context $WORKER_CLUSTER -f ./gloo/workspace-settings-$WORKER_CLUSTER.yaml
kubectl apply --context $WORKER_CLUSTER -f ../../charts/echo/virtual-destination.yaml
