# Deploying Locally

To deploy a single node minikube cluster ensure that you have `kind`, `make`, `glooctl` & `istioctl` 

For kind see https://kind.sigs.k8s.io/docs/user/quick-start/ for more details
For glooctl see //todo
For istioctl see //todo

You also need to set `GLOO_TOKEN` which you can get from the slack channel or me.

Then simply run:
```
make create-cluster-with-gloo
```
or
```
make create-cluster-with-gloo-and-istio
```

in `planetwide-building-society/deployment/cluster`


This will:
* Deploy a new k8s cluster
* Install gloo
* Deploy planetwide
* Register all traffic to route to planetwides gateway 