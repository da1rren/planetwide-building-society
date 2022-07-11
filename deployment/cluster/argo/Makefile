include ../Makefile

CLUSTER_NAME=planetary-core-argo-01

cluster: create-cluster install-argo
	$(info Cluster created)
	$(info You should now be able to access Planetwide at http://localhost:31000/graphql/)

create-cluster: 
	kind create cluster --config kind-cluster.yaml --wait 5m --name $(CLUSTER_NAME)

delete-cluster:
	kind delete cluster --name $(CLUSTER_NAME)

install-argo:
	kubectl create namespace argocd
	kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml