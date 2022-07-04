#!/bin/bash
#
# CLUSTER_CONFIG The kind cluster configuration to use
# CLUSTER_NAME The name to create the cluster with
# METAL_LB_CONFIG The metal lb config to use
#

kind create cluster --config $CLUSTER_CONFIG --wait 5m --name $CLUSTER_NAME
kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.12.1/manifests/namespace.yaml
kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.12.1/manifests/metallb.yaml
docker network inspect -f '{{.IPAM.Config}}' kind
kubectl apply -f $METAL_LB_CONFIG
