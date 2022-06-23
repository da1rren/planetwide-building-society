# Deploying Locally

To deploy a single node minikube cluster ensure that you have `kind` & `make` installed see https://kind.sigs.k8s.io/docs/user/quick-start/ for more details

You also need to set `GLOO_TOKEN` which you can get from the slack channel or me.

Then simply run:
```
make create-cluster
```

This will:
* Deploy a new k8s cluster
* Install gloo
* Deploy planetwide
* Register all traffic to route to planetwides gateway 