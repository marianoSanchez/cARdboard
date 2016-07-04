//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UISprites.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(PlatformSpriteChange), true)]
public class PlatformSpriteChangeEditor : Editor
{
    /// <summary>
    /// Atlas selection callback.
    /// </summary>

    void OnSelectAtlas(Object obj)
    {
        serializedObject.Update();
        SerializedProperty sp = serializedObject.FindProperty("_atlas");
        sp.objectReferenceValue = obj;
        serializedObject.ApplyModifiedProperties();
        NGUITools.SetDirty(serializedObject.targetObject);
        NGUISettings.atlas = obj as UIAtlas;
    }

    /// <summary>
    /// Sprite selection callback function.
    /// </summary>

    void SelectSprite(string spriteName)
    {
        serializedObject.Update();
        SerializedProperty sp = serializedObject.FindProperty("_spriteName");
        sp.stringValue = spriteName;
        serializedObject.ApplyModifiedProperties();
        NGUITools.SetDirty(serializedObject.targetObject);
        NGUISettings.selectedSprite = spriteName;
    }

    /// <summary>
    /// Draw the atlas and sprite selection fields.
    /// </summary>

    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        if (NGUIEditorTools.DrawPrefixButton("Atlas"))
            ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
        SerializedProperty atlas = NGUIEditorTools.DrawProperty("", serializedObject, "_atlas", GUILayout.MinWidth(20f));

        if (GUILayout.Button("Edit", GUILayout.Width(40f)))
        {
            if (atlas != null)
            {
                UIAtlas atl = atlas.objectReferenceValue as UIAtlas;
                NGUISettings.atlas = atl;
                NGUIEditorTools.Select(atl.gameObject);
            }
        }
        GUILayout.EndHorizontal();

        SerializedProperty sp = serializedObject.FindProperty("_spriteName");
        NGUIEditorTools.DrawAdvancedSpriteField(atlas.objectReferenceValue as UIAtlas, sp.stringValue, SelectSprite, false);
    }
}
