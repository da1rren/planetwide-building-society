#!/bin/bash

kubectl delete Workspace --all --all-namespaces --context planetary-adminstration-mesh-01
kubectl delete WorkspaceSettings --all --all-namespaces --context planetary-adminstration-mesh-01

kubectl delete Workspace --all --all-namespaces --context planetary-core-mesh-01
kubectl delete WorkspaceSettings --all --all-namespaces --context planetary-core-mesh-01
kubectl delete VirtualDestination --all -n planetwide --context planetary-core-mesh-01
kubectl delete Routetable --all -n planetwide --context planetary-core-mesh-01

kubectl delete Workspace --all --all-namespaces --context planetary-core-mesh-02
kubectl delete WorkspaceSettings --all --all-namespaces --context planetary-core-mesh-02
kubectl delete VirtualDestination --all -n echoserver --context planetary-core-mesh-02
kubectl delete Routetable --all -n echoserver --context planetary-core-mesh-02

kubectl delete Workspace --all --all-namespaces --context planetary-core-mesh-03
kubectl delete WorkspaceSettings --all --all-namespaces --context planetary-core-mesh-03
kubectl delete VirtualDestination --all -n echoserver --context planetary-core-mesh-03
kubectl delete Routetable --all -n echoserver --context planetary-core-mesh-03

kubectl apply -f gloo/workspaces.yaml --context planetary-adminstration-mesh-01
kubectl apply -f gloo/workspace-settings-planetary-core-mesh-01.yaml --context planetary-core-mesh-01
kubectl apply -f gloo/workspace-settings-planetary-core-mesh-02.yaml --context planetary-core-mesh-02
kubectl apply -f gloo/workspace-settings-planetary-core-mesh-03.yaml --context planetary-core-mesh-03
kubectl apply -f gloo/gateway-planetary-core-mesh-01.yaml --context planetary-core-mesh-01
kubectl apply -f gloo/gateway-planetary-core-mesh-02.yaml --context planetary-core-mesh-02
kubectl apply -f gloo/gateway-planetary-core-mesh-03.yaml --context planetary-core-mesh-03

./scripts/install-aws-echoserver.sh
./scripts/install-az-echoserver.sh
