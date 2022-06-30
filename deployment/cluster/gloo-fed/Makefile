include ../Makefile

GLOO_CONFIGURATION_PATH_01=gloo-configuration-01.yaml
GLOO_CONFIGURATION_PATH_02=gloo-configuration-02.yaml

CLUSTER_NAME_01=planetary-core-gloo-fed-01
CLUSTER_NAME_02=planetary-core-gloo-fed-02

CLUSTER_CONFIG_01=kind-cluster-01.yaml
CLUSTER_CONFIG_02=kind-cluster-02.yaml

clusters:
	kind create cluster --config $(CLUSTER_CONFIG_01) --wait 5m --name $(CLUSTER_NAME_01)
	make GLOO_CONFIGURATION_PATH="$(GLOO_CONFIGURATION_PATH_01)" install-gloo
	make install-istio

	kind create cluster --config $(CLUSTER_CONFIG_02) --wait 5m --name $(CLUSTER_NAME_02)
	make GLOO_CONFIGURATION_PATH="$(GLOO_CONFIGURATION_PATH_02)" install-gloo
	make install-istio

	glooctl cluster register --cluster-name $(CLUSTER_NAME_01) --remote-context kind-$(CLUSTER_NAME_01) \
  		--local-cluster-domain-override host.docker.internal

	glooctl cluster register --cluster-name $(CLUSTER_NAME_02) --remote-context kind-$(CLUSTER_NAME_02) \
  		--local-cluster-domain-override host.docker.internal

delete-clusters:
	kind delete clusters --all


#	glooctl cluster register --cluster-name planetary-core-gloo-fed-01 --remote-context kind-planetary-core-gloo-fed-01 \
#  		--local-cluster-domain-override host.docker.internal