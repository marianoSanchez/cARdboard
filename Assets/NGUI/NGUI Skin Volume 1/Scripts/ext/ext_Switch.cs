using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ext_Switch : MonoBehaviour 
{
	private ext_SwitchButtonEvent _thumb;
	public ext_SwitchButtonEvent Thumb
	{
		get
		{
			if (_thumb == null)
				FindThumb();

			return _thumb;
		}
	}
	
	[SerializeField]
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
            Thumb.State = _state;

            if (EventDelegate.IsValid(onStateChanged))
                EventDelegate.Execute(onStateChanged);
		}
	}

    public bool changeStateOnClick = true;

	public string leftText = "Off";
	public string rightText = "On";
	
	public UILabel lbl_left;
	public UILabel lbl_right;

    public List<EventDelegate> onClicked = new List<EventDelegate>();
    public List<EventDelegate> onStateChanged = new List<EventDelegate>();
    	
	// Use this for initialization
	void Start () 
	{		
		Thumb.State = this._state;
	}	
	
	// Update is called once per frame
	void Update () 
	{
#if UNITY_EDITOR
		this.lbl_left.text = leftText;
		this.lbl_right.text = rightText;
		Thumb.State = this.State;
#endif	
	}

    void FindThumb()
    {
        var spriteComp = gameObject.GetComponentInChildren<UISprite>();
        _thumb = spriteComp.GetComponentInChildren<ext_SwitchButtonEvent>();
    }

    public void ThumbClicked()
    {
        if (EventDelegate.IsValid(onClicked))
            EventDelegate.Execute(onClicked);

        if (changeStateOnClick)
            this.State = !State;
    }
}
