using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwitterProviderAPI : ProviderAPI
{
    #region Variables (private)
        
	#endregion

	#region Propiedades (public)
    
	#endregion

	#region Metodos
    
    public override void Init()
    {
        SPTwitter.Instance.OnTwitterInitedAction += (TWResult result) =>
        {
            if (result.IsSucceeded)
            {
                SocialAPI.Log("Init success.", this);
                if (EventDelegate.IsValid(onInitSuccesfull))
                    EventDelegate.Execute(onInitSuccesfull);
            }
            else
            {
                SocialAPI.LogError("Init failed.", this);
                if (EventDelegate.IsValid(onInitUnsuccesfull))
                    EventDelegate.Execute(onInitUnsuccesfull);
            }
        };

        SPTwitter.Instance.OnPostingCompleteAction += (TWResult result) =>
        {
            if (result.IsSucceeded)
            {
                SocialAPI.Log("Posting success.", this);
                if (EventDelegate.IsValid(onPostingSuccesfull))
                    EventDelegate.Execute(onPostingSuccesfull);
            }
            else
            {
                SocialAPI.LogError("Posting failed.", this);
                if (EventDelegate.IsValid(onPostingUnsuccesfull))
                    EventDelegate.Execute(onPostingUnsuccesfull);
            }
        };

        SPTwitter.Instance.OnAuthCompleteAction += (TWResult result) =>
        {
            if (IsLoggedIn())
            {
                SocialAPI.Log("Login success.", this);
                if (EventDelegate.IsValid(onLoginSuccesfull))
                    EventDelegate.Execute(onLoginSuccesfull);
            }
            else
            {
                SocialAPI.LogError("Login failed.", this);
                if (EventDelegate.IsValid(onLoginUnsuccesfull))
                    EventDelegate.Execute(onLoginUnsuccesfull);
            }
        };

        SPTwitter.Instance.Init();
    }
    
    public override void LogIn()
    {
        SPTwitter.Instance.AuthenticateUser();
    }

    public override void LogOut()
    {
        SPTwitter.Instance.LogOut();
        if (EventDelegate.IsValid(onLogout))
            EventDelegate.Execute(onLogout);
    }

    public override bool IsLoggedIn()
    {
        return SPTwitter.Instance.IsAuthed;
    }

    public override bool IsInited()
    {
        return SPTwitter.Instance.IsInited;
    }

    public override void SharePicture(Texture2D picture, string description)
    {
        SPTwitter.Instance.Post(description, picture);
    }
    
	#endregion
}

