//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UIButton))]
#else
[CustomEditor(typeof(UIButtonChangeSprite), true)]
#endif
public class UIButtonChangeSpriteEditor : Editor
{
    public void DrawProperties ()
    {
        bool dirty = false;
        UIButtonChangeSprite changeSprite = target as UIButtonChangeSprite;

        NGUIEditorTools.SetLabelWidth(120f);
        AnimationOrTween.Trigger trigger = (AnimationOrTween.Trigger)EditorGUILayout.EnumPopup("Trigger condition", changeSprite.trigger);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Agregar Sprites"))
        {
			changeSprite.sprites.Add(new UIButtonSprites());
            dirty = true;
        }
        if (GUILayout.Button("Quitar Sprites") && changeSprite.sprites.Count > 1)
        {
            changeSprite.sprites.RemoveAt(changeSprite.sprites.Count - 1);
            dirty = true;
        }
        GUILayout.EndHorizontal();

        if (!dirty && changeSprite.buttonTarget != null && changeSprite.sprites != null)
        {
            UISprite sprite = changeSprite.buttonTarget.tweenTarget.GetComponent<UISprite>();
            UI2DSprite s2d = changeSprite.buttonTarget.tweenTarget.GetComponent<UI2DSprite>();

            if (sprite != null)
            {
                SerializedProperty sSprites = null;
                for (int i = 0; i < changeSprite.sprites.Count; i++)
                { 
                    if (NGUIEditorTools.DrawHeader("Sprites " + i.ToString(), "Sprites", false, true))
                    {
                        NGUIEditorTools.BeginContents(true);
                        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
                        {
                            SerializedObject obj = new SerializedObject(sprite);
                            obj.Update();
                            SerializedProperty atlas = obj.FindProperty("mAtlas");
                            obj.ApplyModifiedProperties();

                            if (i == 0)
                            {
                                sSprites = serializedObject.FindProperty("sprites").FindPropertyRelative("Array");
                                sSprites.Next(true);
                                sSprites.Next(true);
                            }
                            else
                            {
                                sSprites.Next(false);
                            }

                            NGUIEditorTools.DrawSpriteField("Normal",
                                serializedObject,
                                atlas,
                                sSprites.FindPropertyRelative("normalSprite"));
                            NGUIEditorTools.DrawSpriteField("Hover",
                                serializedObject,
                                atlas,
                                sSprites.FindPropertyRelative("hoverSprite"),
                                true);
                            NGUIEditorTools.DrawSpriteField("Pressed",
                                serializedObject,
                                atlas,
                                sSprites.FindPropertyRelative("pressedSprite"),
                                true);
                            NGUIEditorTools.DrawSpriteField("Disabled",
                                serializedObject,
                                atlas,
                                sSprites.FindPropertyRelative("disabledSprite"),
                                true);
                        }
                        EditorGUI.EndDisabledGroup();

                        NGUIEditorTools.EndContents();
                    }
                }
            }
            else if (s2d != null)
            {
                //if (NGUIEditorTools.DrawHeader("Sprites", "Sprites", false, true))
                //{
                //    NGUIEditorTools.BeginContents(true);
                //    EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
                //    {
                //        SerializedObject obj = new SerializedObject(s2d);
                //        obj.Update();
                //        NGUIEditorTools.DrawProperty("Normal", obj, "mSprite");
                //        obj.ApplyModifiedProperties();

                //        NGUIEditorTools.DrawProperty("Hover", serializedObject, "hoverSprite2D");
                //        NGUIEditorTools.DrawProperty("Pressed", serializedObject, "pressedSprite2D");
                //        NGUIEditorTools.DrawProperty("Disabled", serializedObject, "disabledSprite2D");
                //    }
                //    EditorGUI.EndDisabledGroup();

                //    NGUIEditorTools.EndContents();
                //}
            }
        }

        if (GUI.changed || dirty)
        {
            changeSprite.trigger = trigger;
            EditorUtility.SetDirty(target);
        }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        NGUIEditorTools.SetLabelWidth(120f);

        serializedObject.Update();
        NGUIEditorTools.DrawProperty("Button Target", serializedObject, "buttonTarget");
        DrawProperties();
        serializedObject.ApplyModifiedProperties();		
	}
}
