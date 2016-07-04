using UnityEngine;
using System.Collections;

public class BtnCornerPosition : MonoBehaviour 
{
    public UISprite targetWindow;
    public float yOffset = -30f;

	// Use this for initialization
	void Start () 
    {
        var yPos = targetWindow.localSize.y * .5f;
        yPos += yOffset;
        transform.localPosition = new Vector3(transform.localPosition.x, yPos, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
