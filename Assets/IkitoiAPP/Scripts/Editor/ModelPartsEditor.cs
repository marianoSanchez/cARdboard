using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ModelParts))]
public class ModelPartsEditor : DelegateEditor 
{
    ModelParts _instance;
	
	public void OnEnable()
	{
        _instance = target as ModelParts;
	}

    public override void OnInspectorGUI()
	{		
		if (_instance == null)
			return;

        if (_instance._parts.Length != 8)
            Array.Resize(ref _instance._parts, 8);

		this.DrawDefaultInspector();
    }
}