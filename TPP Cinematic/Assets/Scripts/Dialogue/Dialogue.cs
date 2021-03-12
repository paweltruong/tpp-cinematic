using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
    [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
    /// <summary>
    /// lookup cache for efficiency
    /// </summary>
    Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

    private void OnValidate()
    {
        ReconstructLookup();
    }

    public IEnumerable<DialogueNode> GetAllNodes()
    {
        return nodes;
    }   

    public DialogueNode GetRootNode()
    {
        return nodes[0];
    }

    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
    {
        if (parentNode == null || parentNode.Children == null)
        {
            yield return null;
        }

        if (!nodeLookup.Any())
            Debug.LogError("Look up not created");
        
        foreach (var childId in parentNode.Children)
        {
            if (nodeLookup.ContainsKey(childId))
                yield return nodeLookup[childId];
        }
    }

#if UNITY_EDITOR
    public void CreateNode(DialogueNode parentNode)
    {
        DialogueNode newNode = MakeNode(parentNode);
        Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
        Undo.RecordObject(this, "Added Dialogue Node");
        AddNode(newNode);
    }



    public void DeleteNode(DialogueNode nodeToDelete)
    {
        Undo.RecordObject(this, "Deleted Dialogue Node");
        nodes.Remove(nodeToDelete);
        OnValidate();
        foreach (DialogueNode node in GetAllNodes())
            node.RemoveChild(nodeToDelete.name);

        Undo.DestroyObjectImmediate(nodeToDelete);
    }

    private DialogueNode MakeNode(DialogueNode parentNode)
    {
        var newNode = CreateInstance<DialogueNode>();
        newNode.name = System.Guid.NewGuid().ToString();

        if (parentNode != null)
        {
            parentNode.AddChild(newNode.name);
            //alternating nodes
            newNode.SetAlternatingActor(parentNode.Speaker);
            newNode.SetPosition(parentNode.Rect.position + newNodeOffset);
        }
        else
            newNode.SetActor(ConversationSpeakerKey.Player1);//default
        return newNode;
    }
    private void AddNode(DialogueNode newNode)
    {
        nodes.Add(newNode);
        OnValidate();
    }
#endif

    public void OnBeforeSerialize()
    {

#if UNITY_EDITOR
        if (nodes.Count == 0)
        {
            var firstNode = MakeNode(null);
            AddNode(firstNode);
        }

        //does asset created its file
        if (!String.IsNullOrEmpty(AssetDatabase.GetAssetPath(this)))
        {
            //add nodes as subassets if not saved
            foreach (var node in GetAllNodes())
                if (String.IsNullOrEmpty(AssetDatabase.GetAssetPath(node)))
                    AssetDatabase.AddObjectToAsset(node, this);
        }
#endif
    }

    public void OnAfterDeserialize()
    {
    }

    public void ReconstructLookup()
    {
        nodeLookup.Clear();
        foreach (var node in GetAllNodes())
            if (node != null)
                nodeLookup.Add(node.name, node);
    }
}
