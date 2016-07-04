// Delegate Event Framework
// Copyright: Cratesmith (Kieran Lord)
// Created: 2010
//
// No warranty or garuntee whatsoever with this code. 
// 

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[AddComponentMenu("Delegates/Events/MouseEvents")]
public class MouseEvents : MonoBehaviour
{
	[HideInInspector]
	public SerializableDelegate onMouseEnter;
	
	[HideInInspector]
	public SerializableDelegate onMouseOver;
	
	[HideInInspector]
	public SerializableDelegate onMouseExit;
	
	[HideInInspector]
	public SerializableDelegate onMouseDown;
	
	[HideInInspector]
	public SerializableDelegate onMouseUp;
	
	[HideInInspector]
	public SerializableDelegate onMouseDrag;
	
	public void OnMouseEnter()
	{
		onMouseEnter.Exec(this);
	}
	
	public void OnMouseOver()
	{
		onMouseOver.Exec(this);
	}
	
	public void OnMouseExit()
	{
		onMouseExit.Exec(this);
	}
	
	public void OnMouseDown()
	{
		onMouseDown.Exec(this);
	}
	
	public void OnMouseUp()
	{
		onMouseUp.Exec(this);
	}
	
	public void OnMouseDrag()
	{
		onMouseDrag.Exec(this);
	}
}
