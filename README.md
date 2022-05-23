#  Simple Building Society 

This project currently contains 4 microservices:

* Blazorwasm Web Ui
* Gateway Api
* Accounts Api
* Members Api
* Transactions Api

One member can have many accounts

One account has one member

## Dependencies
* Currently none

## To run
* Run `docker compose up` in the src folder

Some data will be randomly generated and stored in memory.

## Endpoints
* Heathchecks Ui http://localhost:9001/healthchecks-ui
  * This will check all endpoints & databases are available
* Federated Gateway schema explorer http://localhost:9001/graphql

## Sample Queries

List a member with their:
* Basic Details
* Marketing Preferences
* Accounts List
* Transactions

```graphql
query getMemberAccounts {
  member(memberId: "TWVtYmVyCmkx") {
    id
    firstname
    surname
    preferences {
      byPost
      byOnline
    }
    accounts {
      iban
      balance
      transactions {
        id
        amount
        reference
      }
    }
  }
}
```

Batching Queries:

This allows us to request any set of entities we like
```
query getMemberAccounts {
  transactions(accountId: "QWNjb3VudAppMQ=="){
    amount
    reference
  }

  member(memberId: "TWVtYmVyCmkx") {
    preferences {
      byTelephone
      byEmail
    }
  }
}
```

## Sample Mutations
Adds a transaction to memberwith the Id "QWNjb3VudAppMQ=="

```graphql
mutation addTransaction {
  addTransaction(input: { accountId: "QWNjb3VudAppMQ==", amount: -200, reference: "Rent2",  tags: ["house", "spending"] }) {
    transaction {
      id
      accountId
      amount
      reference
      tags
    }
  }
}
```

Batching Mutations & Using Aliases
```
mutation addTransaction {
  transaction1: addTransaction(
    input: {
      accountId: "QWNjb3VudAppMQ=="
      amount: -200
      reference: "Rent2"
      tags: ["house", "spending"]
    }
  ) {
    transaction {
      id
      accountId
      amount
      reference
      tags
    }
  }

  transaction2: addTransaction(
    input: {
      accountId: "QWNjb3VudAppMQ=="
      amount: -333
      reference: "Steam Deck"
      tags: ["entertainment"]
    }
  ) {
    transaction {
      id
      accountId
      amount
      reference
      tags
    }
  }
}

```

## Sample Subscriptions
Triggers when the previous mutation is run.

*Note: This currently does not work via the gateway, support is already built into HC 13 which should release in the next month or two*

```
subscription transactionAdded {
  transactionAdded {
    madeOn
    amount
    reference
    tags
  }
}
```