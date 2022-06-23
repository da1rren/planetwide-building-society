# Some notes about deploying to Gloo

glooctl add route \
  --path-exact /planetwide \
  --dest-name planetwide-gateway \
  --prefix-rewrite /graphql
  --dest-namespace planetwide


  kubectl label service <service_name> discovery.solo.io/function_discovery=enabled

# Install with
glooctl install gateway enterprise --license-key=<<GLOO_TOKEN here>> --values gloo-configuration.yaml

## Thoughts
Native Graphql services not discovered automatically?
* Discovery autogen, 
  * Openapi 
  * Existing graphql server not detected

* Stiching
  * automatically mushes it down
  * Auto-merges graphs 
 
* How do you detect changes to the graph?
  * Changes in stiched schemas are polled automatically
  * adding or removing is in the affected resources

* Governance api
  * graphql inspector
  * Can manage how fields are removed

* Persisted queries
  * Can restrict it works the same way apollo
  * No examples at the moment

* Query
  * Standard rate limiting
  * depth limiting
  * No support for Complexity

* Apollo relay not supportted

* No support at the moment and not planned
  * Defer
  * Stream

* Subscriptions
  * On roadmap, but not supported at the moment
  * Awaiting critical mass of customers

* No bindings for middleware
  * We cannot hook in at the gateway level analyse queries/report metrics

* Testing graphql
  * To test we need to spin up a k8s cluster

* No plans at the moment to support soap/wsl

* WASM extensions
  * Planned but not on the roadmap
  * No hook in points