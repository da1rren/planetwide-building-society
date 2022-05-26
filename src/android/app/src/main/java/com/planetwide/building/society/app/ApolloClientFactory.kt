package com.planetwide.building.society.app

import com.apollographql.apollo3.ApolloClient

class ApolloClientFactory {
    private val graphqlEndpoint = "http://10.0.2.2:9001/graphql";

    fun build(): ApolloClient {
        return ApolloClient.Builder()
            .serverUrl(graphqlEndpoint)
            .build();
    }
}