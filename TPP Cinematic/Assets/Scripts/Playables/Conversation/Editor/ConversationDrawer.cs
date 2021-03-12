using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(ConversationBehaviour))]
public class ConversationDrawer : PropertyDrawer
{
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 1;
        return fieldCount * EditorGUIUtility.singleLineHeight 
            + EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty speakerProp = property.FindPropertyRelative("speaker");

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, speakerProp);

        //TODO:add more settings
    }
}
