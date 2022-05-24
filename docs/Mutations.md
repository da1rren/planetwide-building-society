# Mutations
See the graphql docs https://graphql.org/learn/queries/#mutations

## Adds a transaction to a member

```graphql
mutation addTransaction {
  addTransaction(input: { accountId: "QWNjb3VudAppMQ==", amount: -200, reference: "Rent2",  tags: ["house", "spending"] }) {
    transactionBase {
      id
      accountId
      amount
      reference
      tags
    }
  }
}
```

## Batching Mutations & Using Aliases
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
    transactionBase {
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
    transactionBase {
      id
      accountId
      amount
      reference
      tags
    }
  }
}

```