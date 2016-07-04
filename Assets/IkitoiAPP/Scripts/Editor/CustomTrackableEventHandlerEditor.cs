using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(CustomTrackableEventHandler))]
public class CustomTrackableEventHandlerEditor : DelegateEditor 
{
    public override void OnInspectorGUI()
    {
        DrawEventInspector();
    }
}

