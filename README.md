#  Simple Building Society 

This project currently contains 4 microservices:

* Blazorwasm Web Ui
* Gateway Api
* Accounts Api
* Members Api

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
{
  members(first: 5) {
    nodes{
      firstname
      surname
      accounts {
        id
        sortCode
        number
        balance
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

Filter to accounts that have balances > 5000
```graphql
{
  accounts(first: 5, where: { balance: {gt: 5000}}) {
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