using UnityEngine;
using System.Collections;
using Facebook;

public class BtnSharePicture : MonoBehaviour
{
	#region Variables (private)    

    private ProviderAPI _pAPI;

    private EventDelegate loginSuccessEvent;
    private EventDelegate loginFailedEvent;

    private EventDelegate shareFailedEvent;
    private EventDelegate shareSuccessEvent;
    
	#endregion

	#region Propiedades (public)

    public ProviderAPI API
    {
        get { return _pAPI; }
    }

    public UITexture target;

    public ProviderAPI.ProviderEnum providerType;

    public string message = "";

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (providerType == ProviderAPI.ProviderEnum.FACEBOOK)
            _pAPI = SocialAPI.Facebook;
        else if (providerType == ProviderAPI.ProviderEnum.GOOGLE)
            _pAPI = SocialAPI.Google;
        else if (providerType == ProviderAPI.ProviderEnum.TWITTER)
            _pAPI = SocialAPI.Twitter;
        else if (providerType == ProviderAPI.ProviderEnum.INSTAGRAM)
            _pAPI = SocialAPI.Instagram;

        loginSuccessEvent = new EventDelegate(this, "LoginErrorMessage");
        loginFailedEvent = new EventDelegate(this, "LoginErrorMessage");

        shareFailedEvent = new EventDelegate(this, "ShareErrorMessage");
        shareSuccessEvent = new EventDelegate(this, "ShareSuccessMessage");
    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	void Start ()	
	{
	
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	    
	}

    void OnClick()
    {
        if (target == null) return;

        if (!_pAPI.IsLoggedIn())
        {
            var dialog = UINotificationDialog.CreateDialog(gameObject.GetComponentInParent<UIPanel>().transform);

            loginSuccessEvent.parameters[0].value = dialog;
            loginFailedEvent.parameters[0].value = dialog;

            EventDelegate.Add(_pAPI.onLoginSuccesfull, loginSuccessEvent, true);
            EventDelegate.Add(_pAPI.onLoginUnsuccesfull, loginFailedEvent, true);

            EventDelegate.Add(_pAPI.onLoginSuccesfull, () => SharePictureDialog(), true);
            StartCoroutine(delayedLogIn(dialog.transitionDuration + .05f));
        }

        if (_pAPI.IsLoggedIn())
        {
            SharePictureDialog();
        }
    }

	#endregion

    #region Metodos
    private IEnumerator delayedLogIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _pAPI.LogIn();
    }

    private IEnumerator delayedSharePicture(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _pAPI.SharePicture((Texture2D)target.mainTexture, message);
    }
    
    public void LoginSuccessMessage(UINotificationDialog dialog)
    {
        if (dialog != null)
        {
            dialog.Description = "Logueado con éxito.";
            dialog.Finish();
        }
        EventDelegate.Remove(_pAPI.onLoginUnsuccesfull, loginFailedEvent);
    }

    public void LoginErrorMessage(UINotificationDialog dialog)
    {
        if (dialog != null)
        {
            dialog.Description = "Falló el logueo.";
#if UNITY_IOS
            dialog.Description += "\nVerificar que este la apliación instalada y con un usuario asignado.";
#endif
            dialog.Finish();
        }
        EventDelegate.Remove(_pAPI.onLoginSuccesfull, loginSuccessEvent);
    }

    public void ShareErrorMessage(UINotificationDialog dialog)
    {
        if (dialog != null)
        {
            dialog.Description = "No se pudo compartir la imagen.";
#if UNITY_IOS
            dialog.Description += "\nVerificar que este la apliación instalada y con un usuario asignado.";
#endif
            dialog.Finish();
        }
        EventDelegate.Remove(_pAPI.onPostingSuccesfull, shareSuccessEvent);
    }

    public void ShareSuccessMessage(UINotificationDialog dialog)
    {
        if (dialog != null)
        {
            if (providerType == ProviderAPI.ProviderEnum.INSTAGRAM)
                dialog.Description = "Enviada la captura a Instagram!";
            else
                dialog.Description = "Se compartió la imagen con éxito!";
            dialog.Finish();
        }
        EventDelegate.Remove(_pAPI.onPostingUnsuccesfull, shareFailedEvent);
    }

    void SharePictureDialog()
    {
        var _shareDialog = UIConfirmationDialog.CreateDialog(transform);
        _shareDialog.Description = "¿Esta seguro que desea subir la imagen a " + providerType.ToString() + "?";

        EventDelegate.Add(_shareDialog.onAccept, SharePicture, true);
        _shareDialog.Show();
    }

    void SharePicture()
    {
        var dialog = UINotificationDialog.CreateDialog(gameObject.GetComponentInParent<UIPanel>().transform);

        shareSuccessEvent.parameters[0].value = dialog;
        shareFailedEvent.parameters[0].value = dialog;

        EventDelegate.Add(_pAPI.onPostingUnsuccesfull, shareFailedEvent, true);
        EventDelegate.Add(_pAPI.onPostingSuccesfull, shareSuccessEvent, true);

        StartCoroutine(delayedSharePicture(dialog.transitionDuration + .05f));
    }


	#endregion
}

