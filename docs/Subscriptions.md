# Subscriptions

## On Transaction Added
Triggers when the previous mutation is run.

*Note: This currently does not work via the gateway, support is already built into HC 13 which should release in the next few months*

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

