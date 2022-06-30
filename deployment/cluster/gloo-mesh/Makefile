include ../Makefile

# Admin http://172.18.254.200:8090/

GLOO_CONFIGURATION_PATH_01=gloo-configuration-01.yaml
GLOO_CONFIGURATION_PATH_02=gloo-configuration-02.yaml

CLUSTER_CONFIG_01=kind-cluster-01.yaml
CLUSTER_CONFIG_02=kind-cluster-02.yaml

ADMIN_CLUSTER_NAME=planetary-adminstration-mesh-01
DATA_CLUSTER_NAME=planetary-core-mesh-01

ADMIN_CONTEXT_NAME=kind-$(ADMIN_CLUSTER_NAME)
DATA_CONTEXT_NAME=kind-$(DATA_CLUSTER_NAME)

METAL_LB_CONFIG=SetThisPlox

GLOO_MESH_VERSION=2.0.5

ENDPOINT_GLOO_MESH=gloo-mesh-mgmt-server:9900
HOST_GLOO_MESH=gloo-mesh-mgmt-server

clusters:
	kind create cluster --config $(CLUSTER_CONFIG_02) --wait 5m --name $(DATA_CLUSTER_NAME)
	make METAL_LB_CONFIG=metallb-config-02.yaml install-metalb

	kind create cluster --config $(CLUSTER_CONFIG_01) --wait 5m --name $(ADMIN_CLUSTER_NAME)
	make METAL_LB_CONFIG=metallb-config-01.yaml install-metalb

	make install-solo-istio
	make configure-administration-cluster
	make configure-worker-cluster
	make install-gloo-addons
	make configure-planetwide-for-istio
	kubectl label namespace planetwide istio.io/rev=$(ISTIO_REV)
	make configure-workspaces
	make install-planetwide-core
	make configure-planetwide-gateway

configure-administration-cluster:
	helm repo add gloo-mesh-enterprise https://storage.googleapis.com/gloo-mesh-enterprise/gloo-mesh-enterprise 
	helm repo add gloo-mesh-agent https://storage.googleapis.com/gloo-mesh-enterprise/gloo-mesh-agent
	helm repo update

	kubectl --context $(ADMIN_CONTEXT_NAME) create ns gloo-mesh 

	helm upgrade --install gloo-mesh-enterprise gloo-mesh-enterprise/gloo-mesh-enterprise \
		--namespace gloo-mesh --kube-context $(ADMIN_CONTEXT_NAME) \
		--version=$(GLOO_MESH_VERSION) \
		--set glooMeshMgmtServer.ports.healthcheck=8091 \
		--set glooMeshUi.serviceType=LoadBalancer \
		--set mgmtClusterName=$(ADMIN_CLUSTER_NAME) \
		--set licenseKey=$(GLOO_TOKEN)

configure-worker-cluster:
	kubectl apply --context $(ADMIN_CONTEXT_NAME) -f ./gloo/cluster-registration.yaml

	kubectl --context ${DATA_CONTEXT_NAME} create ns gloo-mesh
	
	ADMIN_CONTEXT_NAME=$(ADMIN_CONTEXT_NAME)   \
		DATA_CONTEXT_NAME=$(DATA_CONTEXT_NAME) \
		DATA_CLUSTER_NAME=$(DATA_CLUSTER_NAME) \
		GLOO_MESH_VERSION=$(GLOO_MESH_VERSION) \
		./gloo/register-cluster.sh 

configure-workspaces:
	kubectl apply --context $(ADMIN_CONTEXT_NAME) -f ./gloo/workspaces.yaml
	kubectl apply --context $(DATA_CONTEXT_NAME) -f ./gloo/workspace-settings.yaml

install-gloo-addons:
	kubectl --context $(DATA_CONTEXT_NAME) create namespace gloo-mesh-addons
	kubectl --context $(DATA_CONTEXT_NAME) label namespace gloo-mesh-addons istio.io/rev=1-13

	helm upgrade --install gloo-mesh-agent-addons gloo-mesh-agent/gloo-mesh-agent \
		--namespace gloo-mesh-addons \
		--kube-context=$(DATA_CONTEXT_NAME) \
		--set glooMeshAgent.enabled=false \
		--set rate-limiter.enabled=true \
		--set ext-auth-service.enabled=true \
		--version $(GLOO_MESH_VERSION)

install-solo-istio: download-istio
	kubectl config use-context $(DATA_CONTEXT_NAME)
	kubectl create ns istio-system
	kubectl create ns istio-gateways

	helm upgrade --install istio-base \
		./istio-$(ISTIO_VERSION)/manifests/charts/base \
		-n istio-system \
		--set defaultRevision=$(ISTIO_REV)

	helm upgrade --install istio-$(ISTIO_VERSION) \
		./istio-$(ISTIO_VERSION)/manifests/charts/istio-control/istio-discovery \
		-n istio-system \
		--values ./istio/istio.yaml 

	kubectl label namespace istio-gateways istio.io/rev=$(ISTIO_REV)

	helm upgrade --install istio-ingressgateway \
		./istio-$(ISTIO_VERSION)/manifests/charts/gateways/istio-ingress \
		-n istio-gateways \
		--values ./istio/istio-ingress.yaml

	make delete-istio

configure-planetwide-gateway:
	kubectl apply --context $(DATA_CONTEXT_NAME) -f ./gloo/planetwide-gateway.yaml

install-metalb:
	kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.12.1/manifests/namespace.yaml
	kubectl apply -f https://raw.githubusercontent.com/metallb/metallb/v0.12.1/manifests/metallb.yaml
	docker network inspect -f '{{.IPAM.Config}}' kind
	kubectl apply -f $(METAL_LB_CONFIG)


delete-clusters:
	kind delete clusters --all
