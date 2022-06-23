delete-cluster:
	kind delete cluster

ifeq ($(GLOO_TOKEN),)
$(info No GLOO_TOKEN env variable found, please set it with a Gloo enterprise license key)
else
$(info GLOO_TOKEN env variable found)

create-cluster:
	kind create cluster --config kind-cluster.yaml --wait 5m
	glooctl install gateway enterprise --license-key=$(GLOO_TOKEN) --values gloo-configuration.yaml
	helm install planetwide ../charts/planetwide --namespace planetwide --create-namespace --wait
	glooctl add route --path-prefix / --dest-name planetwide-planetwide-gateway-80

endif
