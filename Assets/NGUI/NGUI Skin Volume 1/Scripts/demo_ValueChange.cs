using UnityEngine;
using System.Collections;

public class demo_ValueChange : MonoBehaviour 
{

	public float startValue = 0.5f;	
	public float speed = 1.0f;
	public bool pingPong = true;
	
	private float dir = 1.0f;
	private UISlider slider;
	
	// Use this for initialization
	void Start () 
	{
		this.slider = (UISlider)this.gameObject.GetComponentInChildren<UISlider>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        this.slider.value = (this.slider.value + (this.dir * Time.deltaTime * this.speed));
		
		if (pingPong)
		{
            if (this.slider.value >= 0.9f) 
			{
				this.dir = -1.0f;
			}
            if (this.slider.value <= 0.1f)
			{
				this.dir = 1.0f;
			}
		}
		if (!pingPong)
		{
			if (this.slider.value>=1.0f) this.slider.value = 0f;
		}
	}
}
