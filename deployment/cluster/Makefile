# https://4nzr3nswgcav75pijf6lwrahiu0odlmw.lambda-url.eu-west-2.on.aws/

create-cluster:
	kind create cluster --config kind-cluster.yaml --wait 5m --name planetary-core-01

install-gloo:
ifeq ("$(GLOO_TOKEN)",)
	$(info No GLOO_TOKEN env variable found, please set it with a Gloo enterprise license key)
else
	$(info GLOO_TOKEN env variable found)
	glooctl install gateway enterprise --license-key=$(GLOO_TOKEN) --values gloo-configuration.yaml
endif

install-istio:
	curl -L https://istio.io/downloadIstio | ISTIO_VERSION=1.13.5 TARGET_ARCH=x86_64 sh -
	istio-1.13.5/bin/istioctl install -y
	rm -rf istio-1.13.5/
	kubectl create namespace planetwide
	kubectl create -f istio-peer-security-policy.yaml
	kubectl label namespace planetwide istio-injection=enabled
	kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/kiali.yaml
	kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/prometheus.yaml

# See https://github.com/solo-io/gloo/issues/6623
# glooctl istio inject	

install-planetwide:
	helm install planetwide ../charts/planetwide --namespace planetwide --create-namespace --wait
	glooctl add route --path-prefix / --dest-name planetwide-planetwide-gateway-80

create-cluster-with-gloo: create-cluster install-gloo install-planetwide
	$(info Gloo cluster created)

create-cluster-with-gloo-and-istio: create-cluster install-gloo install-istio install-planetwide
	glooctl istio enable-mtls --upstream planetwide-planetwide-gateway-80
	$(info Gloo & istio cluster created)

delete-cluster:
	kind delete cluster --name planetary-core-01
