using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimeMachineClip))]
public class TimeMachineClipInspector : Editor
{
    private SerializedProperty actionProp;

    private void OnEnable()
    {
        actionProp = serializedObject.FindProperty("action");
    }

    public override void OnInspectorGUI()
    {
        bool isMarker = false; //if it's a marker we don't need to draw anything

        //Action
        EditorGUILayout.PropertyField(actionProp);

        //change the int into an enum
        int index = actionProp.enumValueIndex;
        TimeMachineBehaviour.TimeMachineAction actionType = (TimeMachineBehaviour.TimeMachineAction)index;

        //Draws only the appropriate information based on the Action Type
        switch (actionType)
        {
            case TimeMachineBehaviour.TimeMachineAction.Marker:
                isMarker = true;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(TimeMachineClip.markerLabel)));
                break;

            case TimeMachineBehaviour.TimeMachineAction.JumpToMarker:
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(TimeMachineClip.conversation)));
                break;
        }


        if (!isMarker)
        {
            EditorGUILayout.HelpBox("MarkerToJump will be resolved based on the conversation state and following nodes.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
