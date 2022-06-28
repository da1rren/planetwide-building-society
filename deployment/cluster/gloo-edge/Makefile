include ../Makefile

CLUSTER_NAME=planetary-core-gloo-01

cluster: create-cluster install-gloo install-planetwide
	$(info Cluster created)
	$(info You should now be able to access Planetwide at http://localhost:31000/graphql/)

create-cluster: 
	kind create cluster --config kind-cluster.yaml --wait 5m --name $(CLUSTER_NAME)

delete-cluster:
	kind delete cluster --name $(CLUSTER_NAME)
