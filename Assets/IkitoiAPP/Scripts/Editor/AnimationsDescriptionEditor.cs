using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(AnimationsDescription))]
public class AnimationsDescriptionEditor : DelegateEditor 
{
    AnimationsDescription _instance;
	
	public void OnEnable()
	{
        _instance = target as AnimationsDescription;
	}

    public override void OnInspectorGUI()
	{		
		if (_instance == null)
			return;

        var animsCount = _instance.gameObject.GetComponent<Animation>().GetClipCount() + 1;
        if (_instance.gameObject.GetComponent<CustomAnimator>().hasEndingAnimation) animsCount--;
        if (_instance.descArray.Length != animsCount)
            Array.Resize(ref _instance.descArray, animsCount);

		this.DrawDefaultInspector();
    }
}

