#  Simple Building Society 

This project currently contains 4 microservices structured like:

```mermaid
  graph TD;
      A["Gateway"]
      B["Accounts Api"]
      C["Members Api"]
      D["Transactions Api"]
      E["Sqlite In Memory"]
      EE["Sqlite In Memory"]
      F["Mongodb"]
      G["European Central Bank Api"]

      A-->B;
      A-->C;
      A-->D;
      B-->E
      C-->EE
      D-->F
      D-->G
```

Additionally all services use redis to coordinate the graph.

## Why Graphql

* Data Fetching
  * No under or over fetching
* Schema & Type Safety
* Stitching allows us to integrate anything into the graphql

![Why Graphql](docs/images/requests.png "Why Graphql")

## Docs
* [Queries](docs/Queries.md)
* [Mutations](docs/Mutations.md)
* [Subscriptions](docs/Subscriptions.md)

## Endpoints
* Heathchecks Ui http://localhost:9001/healthchecks-ui
  * This will check all endpoints & databases are available
* Federated Gateway schema explorer http://localhost:9001/graphql

## To run
* Run `docker compose up` in the src folder

Some data will be generated and stored in memory.
