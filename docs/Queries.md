# Queries

See the graphql docs https://graphql.org/learn/queries/

![Why Graphql](images/burger.png "Why Graphql")

## Query a member details

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

## Batching Queries:

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

## Interfaces/Polymorphism
As we can use interfaces, we can do type descrimination

```graphql
{
  member(memberId: "TWVtYmVyCmkx") {
    id
    firstname
    accounts {
      id

      iban
      balance
      transactions {
        __typename
        id
        amount
        reference
        tags
        madeOn

        ... on BasicTransaction {
          city
        }

        ... on DirectDebitTransaction {
          merchant
        }
      }
    }
  }
}

```

## Union types

Union types let us merge arrays of directly unrelated objects.

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
        metadata{
          __typename

          ... on NetworkMetadata{
            ipAddress
          }

          ... on LatencyMetadata{
            latency
          }

          ... on RetentionMetadata {
            deleteOn
          }
        }
      }
    }
  }
}
```