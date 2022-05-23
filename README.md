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

Go to http://localhost:9001/graphql/ for the schema explorer or http://localhost:8080/ for a basic ui

## Sample Queries

List the first 5 members and their accounts:

```graphql
query getMemberAccounts {
  member(memberId: "TWVtYmVyCmkx") {
    id
    firstname
    surname
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

List the first 5 accounts and their members details
```graphql
{
  accounts(first: 5) {
    nodes{
      iban
      balance
      member {
        firstname
        surname
      }
    }
  }
}
```
