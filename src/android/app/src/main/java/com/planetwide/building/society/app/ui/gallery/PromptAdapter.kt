package com.planetwide.building.society.app.ui.gallery

import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.planetwide.building.society.app.ApolloClientFactory
import com.planetwide.building.society.app.DismissPromptMutation
import com.planetwide.building.society.app.GetMemberQuery
import com.planetwide.building.society.app.R
import kotlinx.coroutines.runBlocking

class PromptAdapter(private val dataSet: Array<GetMemberQuery.Prompt>) :
    RecyclerView.Adapter<PromptAdapter.ViewHolder>() {

    class ViewHolder(view: View) : RecyclerView.ViewHolder(view) {
        val messageTextView: TextView
        val markAsReadButton: Button

        init {
            // Define click listener for the ViewHolder's View.
            messageTextView = view.findViewById(R.id.promptMessage)
            markAsReadButton = view.findViewById(R.id.buttonMarkAsRead);
        }
    }

    // Create new views (invoked by the layout manager)
    override fun onCreateViewHolder(viewGroup: ViewGroup, viewType: Int): ViewHolder {
        // Create a new view, which defines the UI of the list item
        val view = LayoutInflater.from(viewGroup.context)
            .inflate(R.layout.prompt_row, viewGroup, false)


        return ViewHolder(view)
    }

    // Replace the contents of a view (invoked by the layout manager)
    override fun onBindViewHolder(viewHolder: ViewHolder, position: Int) {
        val prompt = dataSet[position]

        viewHolder.markAsReadButton.setOnClickListener(View.OnClickListener { view ->
            run {
                markAsRead(prompt.id)
            }
        })

        if (prompt.dismissedOn != null) {
            viewHolder.markAsReadButton.text = "Already read."
        }

        viewHolder.messageTextView.text = prompt.message
    }

    fun markAsRead(promptId: String) {
        var factory = ApolloClientFactory();
        val client = factory.build()

        runBlocking {
            var response = client.mutation(DismissPromptMutation(promptId!!))
                .execute();

            if (response.hasErrors()) {
                Log.e("PromptAdapter", "Failed to update: " + promptId);
            } else {
                Log.i("PromptAdapter", "Updated: " + promptId);
            }
        }
    }

    // Return the size of your dataset (invoked by the layout manager)
    override fun getItemCount() = dataSet.size
}

