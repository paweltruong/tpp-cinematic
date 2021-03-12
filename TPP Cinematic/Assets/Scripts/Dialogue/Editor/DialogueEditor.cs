using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
    Dialogue selectedDialogue = null;
    [NonSerialized] GUIStyle nodeStyle;
    [NonSerialized] GUIStyle playerNodeStyle;
    [NonSerialized] DialogueNode draggingNode = null;
    [NonSerialized] Vector2 draggingOffset;
    [NonSerialized] DialogueNode creatingNode;
    [NonSerialized] DialogueNode deletingNode;
    [NonSerialized] DialogueNode linkingParentNode;
    Vector2 scrollPosition;
    [NonSerialized] bool draggingCanvas;
    [NonSerialized] Vector2 draggingCanvasOffset;

    const float canvasSize = 4000f;
    const float backgroundSize = 50f;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    [OnOpenAssetAttribute(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
        if (asset != null)
        {
            ShowEditorWindow();
            return true;
        }
        return false;
    }

    void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChange;

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        nodeStyle.normal.textColor = Color.white;
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        playerNodeStyle = new GUIStyle();
        playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        playerNodeStyle.normal.textColor = Color.white;
        playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
        playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    void OnSelectionChange()
    {
        var selectedObject = Selection.activeObject as Dialogue;
        if (selectedObject != null)
        {
            selectedDialogue = selectedObject;
            Repaint();
        }
    }

    void OnGUI()
    {
        if (selectedDialogue == null)
        {
            EditorGUILayout.LabelField("No Dialogue selected.");
        }
        else
        {
            ProcessEvents();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            var canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
            var backgroundTexture = Resources.Load<Texture2D>("background");
            var textureCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
            GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoords);

            //Draw order
            foreach (var node in selectedDialogue.GetAllNodes())
                DrawNode(node);
            foreach (var node in selectedDialogue.GetAllNodes())
                DrawConnections(node);

            EditorGUILayout.EndScrollView();

            //Add node pending
            if (creatingNode != null)
            {
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }
            //Delete node pending
            if (deletingNode != null)
            {
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }

        }
    }

    /// <summary>
    /// Handles draging
    /// </summary>
    private void ProcessEvents()
    {
        if (Event.current.type == EventType.MouseDown && draggingNode == null)
        {
            var canvasPoint = Event.current.mousePosition + scrollPosition;
            draggingNode = GetNodeAtPoint(canvasPoint);
            if (draggingNode != null)
            {
                draggingOffset = draggingNode.Rect.position - Event.current.mousePosition;
                Selection.activeObject = draggingNode;
            }
            else
            {
                //record drag offset
                draggingCanvas = true;
                draggingCanvasOffset = canvasPoint;

                Selection.activeObject = selectedDialogue;
            }

        }
        else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
        {
            draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
        {
            scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseUp && draggingNode != null)
        {
            draggingNode = null;
        }
        else if (Event.current.type == EventType.MouseUp && draggingCanvas)
        {
            draggingCanvas = false;
        }
    }


    private void DrawNode(DialogueNode node)
    {
        if (node != null)
        {
            GUIStyle style = nodeStyle;
            if (node.Speaker == ConversationSpeakerKey.Player1)
                style = playerNodeStyle;

            GUILayout.BeginArea(node.Rect, style);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(node.name.Length >= 8 ? node.name.Substring(0, 8) : node.name, EditorStyles.whiteLabel, GUILayout.Width(100));
            EditorGUILayout.LabelField(node.Speaker.ToString(), EditorStyles.whiteLabel);
            GUILayout.EndHorizontal();

            node.SetText(EditorGUILayout.TextField(node.Text));

            //Delete
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("X"))
                deletingNode = node;

            //Linking
            DrawLinkButtons(node);

            //Add
            if (GUILayout.Button("+"))
                creatingNode = node;
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }

    private void DrawLinkButtons(DialogueNode node)
    {
        if (linkingParentNode == null)
        {
            if (GUILayout.Button("link"))
                linkingParentNode = node;
        }
        else if (linkingParentNode == node)
        {
            if (GUILayout.Button("cancel"))
                linkingParentNode = null; ;
        }
        else if (linkingParentNode.Children.Contains(node.name))
        {
            if (GUILayout.Button("unlink"))
            {
                linkingParentNode.RemoveChild(node.name);
                linkingParentNode = null;
            }
        }
        else
        {
            if (GUILayout.Button("child"))
            {
                linkingParentNode.AddChild(node.name);
                linkingParentNode = null;
            }
        }
    }

    private void DrawConnections(DialogueNode node)
    {
        if (selectedDialogue == null || node == null)
            return;

        Vector3 startPosition = new Vector2(node.Rect.xMax, node.Rect.center.y);
        foreach (var childNode in selectedDialogue.GetAllChildren(node))
        {
            if (childNode != null)
            {
                Vector3 endPosition = new Vector2(childNode.Rect.xMin, childNode.Rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;

                controlPointOffset.x *= 0.8f;
                controlPointOffset.y = 0;

                Handles.DrawBezier(
                    startPosition, endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f);
            }
        }
    }

    private DialogueNode GetNodeAtPoint(Vector2 point)
    {
        DialogueNode lastFoundNode = null;
        foreach (var node in selectedDialogue.GetAllNodes())
        {
            if (node.Rect.Contains(point))
                lastFoundNode = node;
        }
        return lastFoundNode;
    }
}
