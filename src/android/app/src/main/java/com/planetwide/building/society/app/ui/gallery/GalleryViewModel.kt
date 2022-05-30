package com.planetwide.building.society.app.ui.gallery

import android.util.Log
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.apollographql.apollo3.cache.normalized.watch
import com.planetwide.building.society.app.ApolloClientFactory
import com.planetwide.building.society.app.GetMemberQuery
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.launch

class GalleryViewModel : ViewModel() {

    private val _member = MutableLiveData<GetMemberQuery.Member>()
    val member: LiveData<GetMemberQuery.Member> = _member;

    private val _prompts = MutableLiveData<List<GetMemberQuery.Prompt>>()
    val prompts: LiveData<List<GetMemberQuery.Prompt>> = _prompts;

    fun getAccounts(){
        viewModelScope.launch {
            var factory = ApolloClientFactory();
            val client = factory.build()
            val response =  client.query(GetMemberQuery("TWVtYmVyCmkx"))
                .watch()
                .collect {
§                    Log.i("PromptViewModel", "Refreshing from prompt cache")
                    _member.postValue(it.data?.member!!)
                    _prompts.postValue(it.data?.prompts!!)
                };

        }
    }
}