package com.planetwide.building.society.app

import com.apollographql.apollo3.ApolloClient
import com.apollographql.apollo3.cache.normalized.api.CacheKey
import com.apollographql.apollo3.cache.normalized.api.CacheKeyGenerator
import com.apollographql.apollo3.cache.normalized.api.CacheKeyGeneratorContext
import com.apollographql.apollo3.cache.normalized.api.MemoryCacheFactory
import com.apollographql.apollo3.cache.normalized.normalizedCache

class ApolloClientFactory {
    companion object{
        private const val graphqlEndpoint = "http://10.0.2.2:9001/graphql";
        private val cacheFactory = MemoryCacheFactory(maxSizeBytes = 10 * 1024 * 1024)
        private val cacheKeyGenerator = object : CacheKeyGenerator {
            override fun cacheKeyForObject(obj: Map<String, Any?>, context: CacheKeyGeneratorContext): CacheKey? {
                if(obj.containsKey("id")){
                    return CacheKey(obj["id"] as String)
                }

                return null;
            }
        }

        private val apolloClient = ApolloClient.Builder()
            .serverUrl(graphqlEndpoint)
            .normalizedCache(
                normalizedCacheFactory = cacheFactory,
                cacheKeyGenerator = cacheKeyGenerator
            )
            .build();
    }


    // how to encrypt?
    // https://github.com/apollographql/apollo-kotlin/issues/2185
    // val sqlNormalizedCacheFactory = SqlNormalizedCacheFactory()
    //
    fun build(): ApolloClient {
        return apolloClient;
    }
}