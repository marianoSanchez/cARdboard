using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SocialAPI
{
	#region Variables (private)	
	
    private bool _yielding;

    private ProviderAPI _instagramAPI = null;
    private ProviderAPI _facebookAPI = null;    
    private ProviderAPI _twitterAPI = null;
    private ProviderAPI _googleAPI = null;

    // Singleton
	private static SocialAPI _instance = null;

	#endregion

	#region Propiedades (public)
    	
	public static SocialAPI Instance 
    {
		get 
		{
            if (_instance == null)
                _instance = new SocialAPI();
			
			return _instance;
		}
	}

    public static ProviderAPI Facebook
	{
		get
		{
            if (!Instance._facebookAPI.IsInited())
                throw new System.Exception("Provider facebook not initialized!"); //Instance._facebookAPI.Init();

			return Instance._facebookAPI;
		}
	}

    public static ProviderAPI Google
    {
        get
        {
            if (!Instance._googleAPI.IsInited())
                throw new System.Exception("Provider google not initialized!"); //Instance._googleAPI.Init();

            return Instance._googleAPI;
        }
    }

    public static ProviderAPI Twitter
    {
        get
        {
            if (!Instance._twitterAPI.IsInited())
                throw new System.Exception("Provider twitter not initialized!"); //Instance._twitterAPI.Init();

            return Instance._twitterAPI;
        }
    }

    public static ProviderAPI Instagram
    {
        get
        {
            if (!Instance._instagramAPI.IsInited())
                throw new System.Exception("Provider twitter not initialized!"); //Instance._twitterAPI.Init();

            return Instance._instagramAPI;
        }
    }
    
	#endregion

	#region Metodos

    public SocialAPI()
	{
        _instagramAPI = new InstagramProviderAPI();
        _facebookAPI = new FacebookProviderAPI();
        _twitterAPI = new TwitterProviderAPI();
        _googleAPI = new GoogleProviderAPI();
	}
    
    public IEnumerator InitializeAPIs(bool facebook, bool twitter, bool instagram, bool google)
    {
        if (facebook && !_facebookAPI.IsInited())
        {
#if !UNITY_EDITOR
            _yielding = true;
#endif

            EventDelegate.Add(_facebookAPI.onInitSuccesfull, () => _yielding = false, true);
            EventDelegate.Add(_facebookAPI.onInitUnsuccesfull, () => _yielding = false, true);

            _facebookAPI.Init();

            while (_yielding) yield return new WaitForEndOfFrame();
        }

        if (twitter && !_twitterAPI.IsInited())
        {
#if !UNITY_EDITOR
            _yielding = true;
#endif

            EventDelegate.Add(_twitterAPI.onInitSuccesfull, () => _yielding = false, true);
            EventDelegate.Add(_twitterAPI.onInitUnsuccesfull, () => _yielding = false, true);

            _twitterAPI.Init();

            while (_yielding) yield return new WaitForEndOfFrame();
        }

        if (google && !_googleAPI.IsInited())
        {
#if !UNITY_EDITOR
            _yielding = true;
#endif

            EventDelegate.Add(_googleAPI.onInitSuccesfull, () => _yielding = false, true);
            EventDelegate.Add(_googleAPI.onInitUnsuccesfull, () => _yielding = false, true);

            _googleAPI.Init();

            while (_yielding) yield return new WaitForEndOfFrame();
        }

        if (instagram && !_instagramAPI.IsInited())
        {
            _instagramAPI.Init();
        }
    }

    public static void Log(string msg, object caller)
    {
        Debug.Log("SocialAPI: (" + caller.GetType().ToString() + ") " + msg);
    }

    public static void LogError(string msg, object caller)
    {
        Debug.LogError("SocialAPI: (" + caller.GetType().ToString() + ") " + msg);
    }

	#endregion
}

