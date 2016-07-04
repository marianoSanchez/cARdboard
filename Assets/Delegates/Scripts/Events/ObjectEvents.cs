
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[AddComponentMenu("Delegates/Events/ObjectEvents")]
public class ObjectEvents : MonoBehaviour
{
	[HideInInspector]
	public SerializableDelegate onStart;
	
	[HideInInspector]
	public SerializableDelegate onAwake;
	
	[HideInInspector]
	public SerializableDelegate onEnable;
	
	[HideInInspector]
	public SerializableDelegate onDisable;
	
	public void OnEnable() 	{onEnable.Exec(this);}
	public void OnDisable() {onDisable.Exec(this);}
	public void Start() 	{onStart.Exec(this);}
	public void Awake() 	{onAwake.Exec(this);}
}