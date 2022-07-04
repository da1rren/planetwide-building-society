#!/bin/bash

WORKER_CLUSTER=planetary-core-mesh-02

kubectl apply -f ../../charts/echo/echo.kube.yaml --context $WORKER_CLUSTER
kubectl apply -f ./gloo/workspace-settings-$WORKER_CLUSTER.yaml --context $WORKER_CLUSTER
