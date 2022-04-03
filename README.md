#  Simple Building Society 

This project currently contains 3 microservices:

* The gateway
* Accounts
* Members

One member can have many accounts

One account has one member

## Dependencies

* Currently none

## To run
* Start:
  * Planetwide.Members.Api
  * Planetwide.Accounts.Api
  * Planetwide.Gateway

Some data will be randomly generated and stored in memory.

Go to https://localhost:7228/graphql/ you may need to refresh your schema as this uses stitching rather than federation for simplicity and this can take a few seconds for all the services to converge.

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