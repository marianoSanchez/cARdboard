using UnityEngine;
using System.Collections;

public class PlatformDisable : MonoBehaviour 
{
    public Devices device = Devices.iOS;

	// Use this for initialization
	void Start ()
    {
        if (device == Devices.iOS)
        {
#if UNITY_IOS
            gameObject.SetActive(false);
#endif
        }
        else if (device == Devices.Android)
        {
#if UNITY_ANDROID
            gameObject.SetActive(false);
#endif
        }
        else if (device == Devices.Web)
        {
#if UNITY_WEBPLAYER
            gameObject.SetActive(false);
#endif
        }
        else if (device == Devices.Other)
        {
#if !UNITY_WEBPLAYER && !UNITY_ANDROID && !UNITY_IOS
            gameObject.SetActive(false);
#endif
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public enum Devices
    {
        Android,
        iOS,
        Web,
        Other
    }
}
