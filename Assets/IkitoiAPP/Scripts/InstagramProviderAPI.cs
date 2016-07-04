using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InstagramProviderAPI : ProviderAPI
{
    #region Variables (private)

    private bool _inited = false;
        
	#endregion

	#region Propiedades (public)
    
	#endregion

	#region Metodos
    
    public override void Init()
    {
        SPInstagram.OnPostingCompleteAction += (InstagramPostResult result) =>
        {
            if (result == InstagramPostResult.RESULT_OK)
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

        _inited = true;
    }
    
    public override void LogIn()
    {
        throw new System.NotImplementedException();
    }

    public override void LogOut()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsLoggedIn()
    {
        return true; //throw new System.NotImplementedException();
    }

    public override bool IsInited()
    {
        return _inited; //throw new System.NotImplementedException();
    }

    public override void SharePicture(Texture2D picture, string description)
    {
        SPInstagram.Instance.Share(picture, description);
    }
    
	#endregion
}

