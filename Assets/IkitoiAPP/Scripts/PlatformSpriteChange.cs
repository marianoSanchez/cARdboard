using UnityEngine;
using System.Collections;

public class PlatformSpriteChange : MonoBehaviour 
{

    #region Variables (private)

    public Devices device = Devices.iOS;

    public UISprite target;

    [HideInInspector]
    [SerializeField]
    UIAtlas _atlas;

    [HideInInspector]
    [SerializeField]
    string _spriteName;

    #endregion

    #region Propiedades (public)

    public UIAtlas atlas
    {
        get
        {
            return _atlas;
        }
    }

    public string spriteName
    {
        get
        {
            return _spriteName;
        }
    }

    #endregion

	// Use this for initialization
	void Start () 
    {
	    if (device == Devices.iOS)
        {
#if UNITY_IOS
            ChangeSprite();
#endif
        } 
        else if (device == Devices.Android)
        {
#if UNITY_ANDROID
            ChangeSprite();
#endif
        }
        else if (device == Devices.Web)
        {
#if UNITY_WEBPLAYER
            ChangeSprite();
#endif
        }
        else if (device == Devices.Other)
        {
#if !UNITY_WEBPLAYER && !UNITY_ANDROID && !UNITY_IOS
            ChangeSprite();
#endif
        }          
	}

    private void ChangeSprite()
    {
        target.atlas = _atlas;
        target.spriteName = _spriteName;
    }

    public enum Devices
    {
        Android,
        iOS,
        Web,
        Other
    }
}
