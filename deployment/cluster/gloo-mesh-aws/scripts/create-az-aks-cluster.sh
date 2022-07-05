#!/bin/bash

CLUSTER_NAME=planetary-core-mesh-03

az group create --name $CLUSTER_NAME --location northeurope

az aks create -g $CLUSTER_NAME -n $CLUSTER_NAME --enable-cluster-autoscaler \
    --node-count 1 --min-count 1 --max-count 3

az aks get-credentials --name $CLUSTER_NAME --resource-group $CLUSTER_NAME --context $CLUSTER_NAME
