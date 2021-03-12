using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO:add cancel conversation with f.e Esc
/// <summary>
/// Holds state of dialog, and manages its workflow togeher with appropriate timeline
/// </summary>
public class Conversation : MonoBehaviour
{
    [SerializeField] Conversant[] conversants;
    [SerializeField] Dialogue dialogue;

    DialogueNode currentNode;
    IEnumerable<DialogueNode> currentNodes;
    string selectedOption;
    DialogUI dialogUI;
    bool animationAlreadyTriggered;

    public DialogueNode Current => currentNodes != null? currentNodes.FirstOrDefault() :  currentNode;
    public bool IsWaitingForSelection => currentNodes != null && selectedOption == null;
    public string SelectedOption => selectedOption;



    private void Awake()
    {
        if (dialogue == null || conversants == null && conversants.Length == 0)
            Debug.LogError("Conversation not set up");
        //Fix lookup in final build
        dialogue.ReconstructLookup();
    }

    private void Start()
    {
        dialogUI = FindObjectOfType<DialogUI>();
        if (dialogUI == null)
            Debug.LogError($"{nameof(dialogUI)} not found in the scene");

        dialogUI.onOptionSelected.AddListener(OnOptionSelected);
        currentNode = dialogue.GetRootNode();
    }

    public DialogueNode GetNextNode()
    {
        if (dialogue != null)
        {
            var nextNodeForOption = GetNextNodeForOption();
            if (nextNodeForOption != null)
                return nextNodeForOption;
            else if (currentNode != null && currentNode.Children != null)
                return dialogue.GetAllNodes().FirstOrDefault(n => n.name == currentNode.Children.FirstOrDefault());
        }
        return null;
    }

    public bool IsNextNodeSameSpeaker()
    {
        if (currentNode != null)
        {
            var nextNode = GetNextNode();
            return nextNode != null && nextNode.Speaker == currentNode.Speaker;
        }
        return false;
    }

    public bool IsNextNodeDifferentSpeaker()
    {
        if (currentNode != null)
        {
            var nextNode = GetNextNode();
            return nextNode != null && nextNode.Speaker != currentNode.Speaker;
        }
        return false;
    }
    /// <summary>
    /// is last node in the sequence
    /// </summary>
    /// <returns></returns>
    public bool IsLeafNode()
    {
        return currentNode != null && (currentNode.Children == null || !currentNode.Children.Any());
    }

    string GetText()
    {
        if (currentNode != null)
        {
            return currentNode.Text;
        }
        return string.Empty;
    }    


    public DialogueNode GetSelectedOptionNode()
    {
        if (currentNodes != null && !string.IsNullOrEmpty(selectedOption) && currentNodes.Any(n => n.name == selectedOption))
            return currentNodes.FirstOrDefault(n => n.name == selectedOption);
        return null;
    }

    public DialogueNode GetNextNodeForOption()
    {
        var optionNode = GetSelectedOptionNode();
        if (dialogue != null && optionNode != null && optionNode.Children != null)
        {
            return dialogue.GetAllNodes().FirstOrDefault(n => n.name == optionNode.Children.FirstOrDefault());
        }
        return null;
    }

    public void DisplayCurrent()
    {
        if (currentNodes != null)
        {
            DisplayCurrentOptions();
        }
        else if (currentNode != null)
        {
            DisplayCurrentText();
        }
    }

    void DisplayCurrentText()
    {
        if (currentNode != null)
        {
            var conversant = GetConversant(currentNode);
            dialogUI.DisplayMessage(conversant.DisplayName, currentNode.Text, conversant.Color);
            if (!animationAlreadyTriggered)
            {
                conversant.PlayAnimation(currentNode.Animation);
                animationAlreadyTriggered = true;
            }
        }
    }

    void DisplayCurrentOptions()
    {
        if (currentNodes != null && currentNodes.Any())
        {
            var conversant = GetConversant(currentNodes.First());
            var responses = new Dictionary<string, string>();
            foreach (var node in currentNodes)
                responses.Add(node.name, node.Text);
            dialogUI.ResetDialog();
            dialogUI.BindResponses(responses);
        }
        else
            Debug.LogError("No options");
    }

    public void SetAndDisplayNextNode()
    {
        var nextNode = GetNextNode();
        if (nextNode != null)
        {
            animationAlreadyTriggered = false;

            if (nextNode.Type == DialogueNodeType.Option)
            {
                var childrenNodes = dialogue.GetAllChildren(currentNode);
                if (childrenNodes == null)
                    Debug.LogError("Option children cannot by null when next one is found");
                if (childrenNodes.Any(ch => ch.Type != DialogueNodeType.Option))
                    Debug.LogError("Dialogue composition invalid, all children have to be same type");
                currentNodes = childrenNodes;
                currentNode = null;
                selectedOption = null;
                DisplayCurrentOptions();
            }
            else
            {
                selectedOption = null;
                currentNodes = null;
                currentNode = nextNode;
                DisplayCurrentText();
            }
        }
    }

    void OnOptionSelected(string nodeUid)
    {
        selectedOption = nodeUid;

        var selectedNode = GetSelectedOptionNode();
        if (!animationAlreadyTriggered)
        {
            var conversant = GetConversant(selectedNode);
            conversant.PlayAnimation(selectedNode.Animation);
            animationAlreadyTriggered = true;
        }
    }

    Conversant GetConversant(DialogueNode node)
    {
        return conversants.FirstOrDefault(c => c.Speaker == node.Speaker);
    }

    internal void DisplayDefault()
    {
        dialogUI.ResetDialog();
    }
}
