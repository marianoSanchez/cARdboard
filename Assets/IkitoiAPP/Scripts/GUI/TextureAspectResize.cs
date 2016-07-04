using UnityEngine;
using System.Collections;

public class TextureAspectResize : MonoBehaviour 
{
    public float minAR = 1.3333f;
    public float maxAR = 1.7777f;

    public UIBasicSprite target;
    
	// Use this for initialization
	void Awake () 
    {
        var targetCamera = Camera.main;
        target.height = (int) (target.width / Mathf.Clamp(targetCamera.aspect, minAR, maxAR));
        target.MarkAsChanged();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
