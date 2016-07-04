using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomAnimator))]
public class CustomAnimatorEditor : DelegateEditor 
{
	CustomAnimator _instance;
	PropertyField[] _fields;
	
	public void OnEnable()
	{
		_instance = target as CustomAnimator;
		_fields = ExposeProperties.GetProperties(_instance);
	}

    public override void OnInspectorGUI()
	{		
		if (_instance == null)
			return;
		
		this.DrawDefaultInspector();

		ExposeProperties.Expose(_fields);

		NGUIEditorTools.SetLabelWidth(80f);
		GUILayout.Space(3f);
		NGUIEditorTools.DrawEvents("On Anim Change", _instance, _instance.onAnimIndexChange);
		GUILayout.Space(3f);
		NGUIEditorTools.DrawEvents("On Forward", _instance, _instance.onForward);
		GUILayout.Space(3f);
		NGUIEditorTools.DrawEvents("On Backward", _instance, _instance.onBackward);
		GUILayout.Space(3f);
		NGUIEditorTools.DrawEvents("On Skipped", _instance, _instance.onSkip);
		GUILayout.Space(3f);
		NGUIEditorTools.DrawEvents("On Rewind", _instance, _instance.onRewind);
		GUILayout.Space(3f);
		NGUIEditorTools.DrawEvents("On Finished", _instance, _instance.onFinished);
    }
}

