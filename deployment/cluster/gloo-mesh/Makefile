include ../Makefile

# Admin http://172.18.254.200:8090/

GLOO_CONFIGURATION_PATH_01=gloo-configuration-01.yaml
GLOO_CONFIGURATION_PATH_02=gloo-configuration-02.yaml

CLUSTER_CONFIG_01=kind-cluster-01.yaml
CLUSTER_CONFIG_02=kind-cluster-02.yaml
CLUSTER_CONFIG_03=kind-cluster-03.yaml

ADMIN_CLUSTER_NAME=planetary-adminstration-mesh-01
DATA_CLUSTER_NAME=planetary-core-mesh-01
PASSIVE_CLUSTER_NAME=planetary-core-mesh-02

ADMIN_CONTEXT_NAME=kind-$(ADMIN_CLUSTER_NAME)
DATA_CONTEXT_NAME=kind-$(DATA_CLUSTER_NAME)
PASSIVE_CONTEXT_NAME=kind-$(PASSIVE_CLUSTER_NAME)

METAL_LB_CONFIG_01=metallb-config-01.yaml
METAL_LB_CONFIG_02=metallb-config-02.yaml
METAL_LB_CONFIG_03=metallb-config-03.yaml


GLOO_MESH_VERSION=2.0.5

ENDPOINT_GLOO_MESH=gloo-mesh-mgmt-server:9900
HOST_GLOO_MESH=gloo-mesh-mgmt-server

clusters:
	METAL_LB_CONFIG=$(METAL_LB_CONFIG_01) \
		CLUSTER_NAME=$(PASSIVE_CLUSTER_NAME) \
		CLUSTER_CONFIG=$(CLUSTER_CONFIG_01) \
		./scripts/create-cluster.sh

	METAL_LB_CONFIG=$(METAL_LB_CONFIG_02) \
		CLUSTER_NAME=$(DATA_CLUSTER_NAME) \
	 	CLUSTER_CONFIG=$(CLUSTER_CONFIG_02) \
		./scripts/create-cluster.sh

	METAL_LB_CONFIG=$(METAL_LB_CONFIG_03) \
		CLUSTER_NAME=$(ADMIN_CLUSTER_NAME) \
		CLUSTER_CONFIG=$(CLUSTER_CONFIG_03) \
		./scripts/create-cluster.sh

	CONTEXT_NAME=$(DATA_CLUSTER_NAME) \
		ISTIO_VERSION=$(ISTIO_VERSION) \
		ISTIO_REV=$(ISTIO_REV) \
		./scripts/install-solo-istio.sh

	CONTEXT_NAME=$(PASSIVE_CLUSTER_NAME) \
		ISTIO_VERSION=$(ISTIO_VERSION) \
		ISTIO_REV=$(ISTIO_REV) \
		./scripts/install-solo-istio.sh

	ADMIN_CLUSTER_NAME=$(ADMIN_CLUSTER_NAME) \
		ADMIN_CONTEXT_NAME=$(ADMIN_CONTEXT_NAME) \
		GLOO_MESH_VERSION=$(GLOO_MESH_VERSION) \
		GLOO_TOKEN=$(GLOO_TOKEN) \
		./scripts/configure-admin-cluster.sh

	ADMIN_CONTEXT_NAME=$(ADMIN_CONTEXT_NAME) \
		DATA_CONTEXT_NAME=$(DATA_CONTEXT_NAME) \
		DATA_CLUSTER_NAME=$(DATA_CLUSTER_NAME) \
		GLOO_MESH_VERSION=$(GLOO_MESH_VERSION) \
		./scripts/configure-worker-cluster.sh

	ADMIN_CONTEXT_NAME=$(ADMIN_CONTEXT_NAME) \
		DATA_CONTEXT_NAME=$(PASSIVE_CONTEXT_NAME) \
		DATA_CLUSTER_NAME=$(PASSIVE_CLUSTER_NAME) \
		GLOO_MESH_VERSION=$(GLOO_MESH_VERSION) \
		./scripts/configure-worker-cluster.sh

	kubectl config use-context $(DATA_CONTEXT_NAME)
	
	DATA_CONTEXT_NAME=$(DATA_CONTEXT_NAME) \
		GLOO_MESH_VERSION=$(GLOO_MESH_VERSION) \
		./scripts/install-gloo-addons.sh


install-gloo-mesh-planetwide:
	make configure-planetwide-for-istio
	kubectl label namespace planetwide istio.io/rev=$(ISTIO_REV)

	kubectl apply -f ../../charts/echo/echo.kube.yaml --context $(PASSIVE_CONTEXT_NAME)

	make configure-workspaces
	make install-planetwide-core
	kubectl apply -f ../../charts/echo/virtual-destination.yaml --context $(DATA_CONTEXT_NAME)

	make configure-planetwide-gateway


configure-workspaces:
	kubectl apply --context $(ADMIN_CONTEXT_NAME) -f ./gloo/workspaces.yaml
	kubectl apply --context $(DATA_CONTEXT_NAME) -f ./gloo/workspace-settings-01.yaml
	kubectl apply --context $(PASSIVE_CONTEXT_NAME) -f ./gloo/workspace-settings-02.yaml


configure-planetwide-gateway:
	kubectl apply --context $(DATA_CONTEXT_NAME) -f ./gloo/planetwide-gateway.yaml

delete-clusters:
	kind delete clusters --all
