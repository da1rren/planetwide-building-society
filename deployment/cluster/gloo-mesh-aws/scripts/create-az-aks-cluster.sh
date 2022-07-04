#!/bin/bash

CLUSTER_NAME=planetary-core-mesh-03

az group create --name $CLUSTER_NAME --location uksouth

az aks create -g $CLUSTER_NAME -n $CLUSTER_NAME --enable-managed-identity --node-count 3 \
    --enable-addons monitoring

az aks get-credentials --name $CLUSTER_NAME --resource-group $RESOURCE_GROUP --context $CLUSTER_NAME
