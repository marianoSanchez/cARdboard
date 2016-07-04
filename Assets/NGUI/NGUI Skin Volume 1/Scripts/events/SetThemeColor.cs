using UnityEngine;
using System.Collections;

public class SetThemeColor : MonoBehaviour 
{

	public void SetR()
	{
		GetThemeColor.COLOR_R = (byte)Mathf.RoundToInt(255*UISlider.current.value);
	}
	
	public void SetG()
	{
		GetThemeColor.COLOR_G = (byte)Mathf.RoundToInt(255*UISlider.current.value);
	}
	
	public void SetB()
	{
		GetThemeColor.COLOR_B = (byte)Mathf.RoundToInt(255*UISlider.current.value);
	}
	
}
