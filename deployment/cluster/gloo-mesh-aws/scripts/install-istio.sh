#!/bin/bash

source .env

curl -L https://istio.io/downloadIstio | ISTIO_VERSION=$ISTIO_VERSION TARGET_ARCH=x86_64 sh -

time xargs -n4 -P 5 -I {} sh -c 'eval "$1"' - {} <<EOF
CONTEXT_NAME=$WORKER_CLUSTER_01 ISTIO_VERSION=$ISTIO_VERSION ISTIO_REV=$ISTIO_REV ./scripts/cluster-install-solo-istio.sh
CONTEXT_NAME=$WORKER_CLUSTER_02 ISTIO_VERSION=$ISTIO_VERSION ISTIO_REV=$ISTIO_REV ./scripts/cluster-install-solo-istio.sh
CONTEXT_NAME=$WORKER_CLUSTER_03 ISTIO_VERSION=$ISTIO_VERSION ISTIO_REV=$ISTIO_REV ./scripts/cluster-install-solo-istio.sh
EOF

rm -rf istio-$ISTIO_VERSION/
