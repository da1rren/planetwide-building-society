package com.planetwide.building.society.app.ui.gallery

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.planetwide.building.society.app.GetMemberQuery
import com.planetwide.building.society.app.databinding.FragmentGalleryBinding

class GalleryFragment : Fragment() {

    private var _binding: FragmentGalleryBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val promptsViewModel =
            ViewModelProvider(this).get(GalleryViewModel::class.java)

        _binding = FragmentGalleryBinding.inflate(inflater, container, false)
        val root: View = binding.root

        val textView: TextView = binding.textGallery
        binding.recyclerViewPrompts.adapter = PromptAdapter(arrayOf(GetMemberQuery.Prompt("none", "Loading...", null)))

        promptsViewModel.member.observe(viewLifecycleOwner) {
            textView.text = "name: " + it.firstname + it.surname
        }

        promptsViewModel.prompts.observe(viewLifecycleOwner) {
            binding.recyclerViewPrompts.adapter = PromptAdapter(it.toTypedArray())
        }

        promptsViewModel.getAccounts();

        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {

    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}