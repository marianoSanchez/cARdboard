using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ext_Switch))]
public class SocialLogginSwitch : MonoBehaviour
{
	#region Variables (private)

    private ProviderAPI _pAPI;
    private ext_Switch _switch;

    private EventDelegate failedEvent;
    private EventDelegate successEvent;
    
	#endregion

	#region Propiedades (public)

    public ProviderAPI.ProviderEnum socialProvider;

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (socialProvider == ProviderAPI.ProviderEnum.FACEBOOK)
            _pAPI = SocialAPI.Facebook;
        else if (socialProvider == ProviderAPI.ProviderEnum.GOOGLE)
            _pAPI = SocialAPI.Google;
        else if (socialProvider == ProviderAPI.ProviderEnum.TWITTER)
            _pAPI = SocialAPI.Twitter;
        else if (socialProvider == ProviderAPI.ProviderEnum.INSTAGRAM)
            _pAPI = SocialAPI.Instagram;

        if (_switch == null)
            _switch = GetComponentInChildren<ext_Switch>();
        
        EventDelegate.Add(_switch.onClicked, () =>
        {
            if (_switch.State)
            {       
                _pAPI.LogOut();
            }
            else
            {
                var dialog = UINotificationDialog.CreateDialog(gameObject.GetComponentInParent<UIPanel>().transform);
                
                failedEvent.parameters[0].value = dialog;
                successEvent.parameters[0].value = dialog;

                EventDelegate.Add(_pAPI.onLoginSuccesfull, successEvent, true);
                EventDelegate.Add(_pAPI.onLoginUnsuccesfull, failedEvent, true);

                StartCoroutine(delayedLogIn(dialog.transitionDuration));
            }
        });

        // Crea y asigna eventos
        failedEvent = new EventDelegate(this, "LoginErrorMessage");
        successEvent = new EventDelegate(this, "LoginSuccessMessage");

        EventDelegate.Add(_pAPI.onLoginSuccesfull, LoggedInUI);        
        EventDelegate.Add(_pAPI.onLoginUnsuccesfull, LoggedOutUI);

        EventDelegate.Add(_pAPI.onLogout, LoggedOutUI);
    }

    void OnDestroy()
    {
        EventDelegate.Remove(_pAPI.onLoginSuccesfull, LoggedInUI);
        EventDelegate.Remove(_pAPI.onLoginUnsuccesfull, LoggedOutUI);

        EventDelegate.Remove(_pAPI.onLogout, LoggedOutUI);
    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	void Start ()	
	{
		CheckLogginStatus();
	}

    void OnEnable()
    {
        CheckLogginStatus();
    }

	#endregion

    #region Metodos

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
        EventDelegate.Remove(_pAPI.onLoginSuccesfull, successEvent);
    }

    public void LoginSuccessMessage(UINotificationDialog dialog)
    {
        if (dialog != null)
        {
            dialog.Description = "Logueado con éxito.";
            dialog.Finish();
        }
        EventDelegate.Remove(_pAPI.onLoginUnsuccesfull, failedEvent);
    }

    private IEnumerator delayedLogIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _pAPI.LogIn();
    }

	public void CheckLogginStatus()
	{
        if (_pAPI.IsLoggedIn())
		{
			LoggedInUI();
		}
		else
		{
			LoggedOutUI();
		}
	}

	public void LoggedOutUI()
	{
        _switch.State = false;
	}

	public void LoggedInUI()
    {
        _switch.State = true;
	}

	#endregion
}