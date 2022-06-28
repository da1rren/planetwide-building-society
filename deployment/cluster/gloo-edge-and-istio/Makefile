include ../Makefile

CLUSTER_NAME=planetary-core-gloo-istio-01

cluster: create-cluster install-istio install-gloo install-planetwide
	glooctl istio inject
	kubectl set image deployment/gateway-proxy sds=quay.io/solo-io/sds:1.11.16 -n gloo-system
	glooctl istio enable-mtls --upstream planetwide-planetwide-gateway-80
	$(info Cluster created)
	$(info You should now be able to access Planetwide at http://localhost:32000/graphql/)

create-cluster: 
	kind create cluster --config kind-cluster.yaml --wait 5m --name $(CLUSTER_NAME)
	
delete-cluster:
	kind delete cluster --name $(CLUSTER_NAME)
