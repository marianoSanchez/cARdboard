using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ProviderAPI
{
    #region Variables (private)
        
	#endregion

    #region Propiedades (public)

    public List<EventDelegate> onInitSuccesfull = new List<EventDelegate>();

    public List<EventDelegate> onInitUnsuccesfull = new List<EventDelegate>();
        
	public List<EventDelegate> onLoginSuccesfull = new List<EventDelegate>();

	public List<EventDelegate> onLoginUnsuccesfull = new List<EventDelegate>();

    public List<EventDelegate> onPostingSuccesfull = new List<EventDelegate>();

    public List<EventDelegate> onPostingUnsuccesfull = new List<EventDelegate>();
	
	public List<EventDelegate> onLogout = new List<EventDelegate>();

	#endregion

	#region Metodos

    public abstract void Init();
    
    public abstract bool IsLoggedIn();

    public abstract bool IsInited();

    public abstract void LogIn();

    public abstract void LogOut();

    public abstract void SharePicture(Texture2D picture, string description);
    
    public enum ProviderEnum
    {
        INSTAGRAM,
        FACEBOOK,
        GOOGLE,
        TWITTER
    }

	#endregion
}

