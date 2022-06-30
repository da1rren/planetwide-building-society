GLOO_CONFIGURATION_PATH=gloo-configuration.yaml

ISTIO_REV=1-13
ISTIO_VERSION=1.13.4
ISTIO_SECURITY_POLICY=istio-peer-security-policy.yaml

install-gloo:
ifeq ("$(GLOO_TOKEN)",)
	$(info No GLOO_TOKEN env variable found, please set it with a Gloo enterprise license key)
else
	$(info GLOO_TOKEN env variable found)
	glooctl install gateway enterprise --license-key=$(GLOO_TOKEN) --values $(GLOO_CONFIGURATION_PATH)
endif

download-istio:
	curl -L https://istio.io/downloadIstio | ISTIO_VERSION=$(ISTIO_VERSION) TARGET_ARCH=x86_64 sh -

delete-istio:
	rm -rf istio-$(ISTIO_VERSION)/

install-istio: download-istio
	istio-$(ISTIO_VERSION)/bin/istioctl install -y --set profile=minimal
	make delete-istio
	make configure-planetwide-for-istio
	kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/kiali.yaml
	kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/prometheus.yaml

# See https://github.com/solo-io/gloo/issues/6623
# glooctl istio inject	

configure-planetwide-for-istio:
	kubectl create namespace planetwide
	kubectl create -f $(ISTIO_SECURITY_POLICY)
	kubectl label namespace planetwide istio-injection=enabled
	kubectl label namespace planetwide istio.io/rev=$(ISTIO_REV)

install-planetwide-core:
	helm install planetwide ../../charts/planetwide --namespace planetwide --create-namespace --wait

install-planetwide: install-planetwide-core
	glooctl add route --path-prefix / --dest-name planetwide-planetwide-gateway-80
