# Deploying Locally

To deploy a single node minikube cluster ensure that you have `kind`, `make`, `glooctl`

For kind see https://kind.sigs.k8s.io/docs/user/quick-start/ for more details
For glooctl do:
```
    curl -sL https://run.solo.io/gloo/install | sh
    export PATH=$HOME/.gloo/bin:$PATH
```

You also need to set `GLOO_TOKEN` which you can get from the slack channel or me.

Then simply run:
```
make cluster
```

in `planetwide-building-society/deployment/cluster/<<Cluster type here>>`


This will:
* Deploy a new k8s cluster
* Install gloo
* Configure it to the requested type of cluster 