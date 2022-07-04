include ../Makefile

clusters:
	./scripts/create-clusters.sh
	./scripts/install-istio.sh
	./scripts/configure-admin-cluster.sh
	./scripts/configure-worker-clusters.sh
	./scripts/install-echoserver.sh
	./scripts/install-planetwide.sh

	# config worker cluster
	# Install planetwide
	# Install echo server


delete-clusters:
	./scripts/delete-clusters.sh
