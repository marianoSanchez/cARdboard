using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacebookProviderAPI : ProviderAPI
{
    #region Variables (private)
        
	#endregion

	#region Propiedades (public)
    
	#endregion

	#region Metodos
    
    public override void Init()
    {
        SPFacebook.Instance.OnInitCompleteAction += () =>
        {
            if (SPFacebook.Instance.IsInited)
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

        SPFacebook.Instance.OnPostingCompleteAction += (FBPostResult result) =>
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

        SPFacebook.Instance.OnAuthCompleteAction += (FB_APIResult result) =>
        {
            if (SPFacebook.Instance.IsLoggedIn)
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

        SPFacebook.Instance.OnLogOut += () =>
        {
            SocialAPI.Log("Logout success.", this);
            if (EventDelegate.IsValid(onLogout))
                EventDelegate.Execute(onLogout);
        };

        SPFacebook.Instance.Init();
    }
    
    public override void LogIn()
    {
        SPFacebook.Instance.Login();
    }

    public override void LogOut()
    {
        SPFacebook.Instance.Logout();
    }

    public override bool IsLoggedIn()
    {
        return SPFacebook.Instance.IsLoggedIn;
    }

    public override bool IsInited()
    {
        return SPFacebook.Instance.IsInited;
    }

    public override void SharePicture(Texture2D picture, string description)
    {
        SPFacebook.Instance.PostImage(description, picture);
    }
    
	#endregion
}

