using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TweenPosition))]
public class ext_SwitchButtonEvent : MonoBehaviour
{
	public float xOffset = 25;	

	private bool _state = false;
    public bool State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            TweenPos.Play(_state);
        }
    }

	private TweenPosition _tweenPos;
    private TweenPosition TweenPos
    {
        get
        {
            if (_tweenPos == null)
                _tweenPos = this.gameObject.GetComponent<TweenPosition>();

            return _tweenPos;
        }
    }
    private ext_Switch _switchParent;
    private ext_Switch SwitchParent
    {
        get
        {
            if (_switchParent == null)
                _switchParent = this.transform.parent.gameObject.GetComponent<ext_Switch>();

            return _switchParent;
        }
    }
    	
	void Awake()
	{
		_tweenPos = this.gameObject.GetComponent<TweenPosition>();	
        _switchParent = this.transform.parent.gameObject.GetComponent<ext_Switch>();

		TweenPos.ResetToBeginning();
	}

    // Use this for initialization
	void Start()
	{
	}
	
	void OnClick() 
	{
        SwitchParent.ThumbClicked();
	}
	
	public void ToggleState()
	{
		State = !State;
	}
	
	public void SetStateOn()
	{
		State = true;
	}
	
	public void SetStateOff()
	{
		State = false;
	}	
}
