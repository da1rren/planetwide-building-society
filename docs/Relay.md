# Relay

Relay is a JavaScript framework for building data-driven React applications with GraphQL, which is developed and used by Facebook.

As part of a specification Relay proposes some schema design principles for GraphQL servers in order to more efficiently fetch, refetch and cache entities on the client. In order to get the most performance out of Relay our GraphQL server needs to abide by these principles.

Basically it gives us globally unique identifiers regardless of the real backing type