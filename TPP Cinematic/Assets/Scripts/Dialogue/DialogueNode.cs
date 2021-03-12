using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueNode : ScriptableObject
{
    [SerializeField] ConversationSpeakerKey speaker;
    [SerializeField] DialogueNodeType type = DialogueNodeType.Text;
    [SerializeField] string text;
    [SerializeField] ConversationAnimation animation = ConversationAnimation.Idle;
    [SerializeField] List<string> children = new List<string>();
    [SerializeField] Rect rect = new Rect(0, 0, 200, 100);

    public Rect Rect => rect;
    public string Text => text;
    public List<string> Children => children;
    public DialogueNodeType Type => type;

    public ConversationSpeakerKey Speaker => speaker;
    public ConversationAnimation Animation => animation;


#if UNITY_EDITOR
    public void SetPosition(Vector2 position)
    {
        Undo.RecordObject(this, "Move Dialogue Node");
        rect.position = position;
        EditorUtility.SetDirty(this);
    }

    public void SetSpeaker(ConversationSpeakerKey speaker)
    {
        if (this.speaker != speaker)
        {
            Undo.RecordObject(this, "Update Dialogue Node Speaker");
            this.speaker = speaker;
            EditorUtility.SetDirty(this);
        }
    }
    public void SetAnimation(ConversationAnimation animation)
    {
        if (this.animation != animation)
        {
            Undo.RecordObject(this, "Update Dialogue Node Animation");
            this.animation = animation;
            EditorUtility.SetDirty(this);
        }
    }
    public void SetType(DialogueNodeType type)
    {
        if (this.type != type)
        {
            Undo.RecordObject(this, "Update Dialogue Node Type");
            this.type = type;
            EditorUtility.SetDirty(this);
        }
    }
    public void SetText(string newText)
    {
        if (text != newText)
        {
            Undo.RecordObject(this, "Update Dialogue Node Text");
            text = newText;
            EditorUtility.SetDirty(this);
        }
    }

    public void AddChild(string childId)
    {
        Undo.RecordObject(this, "Add Dialogue Node Link");
        children.Add(childId);
        EditorUtility.SetDirty(this);
    }
    public void RemoveChild(string childId)
    {
        Undo.RecordObject(this, "Remove Dialogue Node Link");
        children.Remove(childId);
        EditorUtility.SetDirty(this);
    }
    public void SetActor(ConversationSpeakerKey value)
    {
        Undo.RecordObject(this, "Modified Dialogue Node IsPlayerSpeaking");
        speaker = value;
        EditorUtility.SetDirty(this);
    }
    public void SetAlternatingActor(ConversationSpeakerKey parentActor)
    {
        Undo.RecordObject(this, "Modified Dialogue Node IsPlayerSpeaking");
        if (parentActor == ConversationSpeakerKey.Player1)
            speaker = ConversationSpeakerKey.Npc1;
        else
            speaker = ConversationSpeakerKey.Player1;
        EditorUtility.SetDirty(this);
    }
#endif
}
