using UnityEngine;
using System.Collections;

public class UIRenderToggle : MonoBehaviour
{
    private enum OnStart
    {
        DoNothing,
        Hide,
        Show
    }

	#region Variables (private)

    [Header("UI Types")]
    [SerializeField]
    private bool _label;
    [SerializeField]
    private bool _sprite;
    [SerializeField]
    private bool _uiTextures;

    [Space(15)]
    [SerializeField]
    private bool _affectChilds;
    [SerializeField]
    private OnStart _onStart = OnStart.DoNothing;

	#endregion

	#region Propiedades (public)

    public bool AffectChilds
    {
        get
        {
            return _affectChilds;
        }
        set
        {
            _affectChilds = value;
        }
    }

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {

    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	void Start ()
    {
        if (_onStart == OnStart.DoNothing) return;

        else if (_onStart == OnStart.Show)
        {
            Show();
        }
        else if (_onStart == OnStart.Hide)
        {
            Hide();
        }
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update ()
    {

	}

	#endregion

	#region Metodos

    private bool CheckEnabled<T>() where T:MonoBehaviour
    {
        var showing = false;
        var UI = gameObject.GetComponent<T>();
        if (UI != null)
            showing = UI.enabled;

        return showing;
    }

    private void Hide<T>() where T:MonoBehaviour
    {
        //Debug.Log("Ocultando la UI: \"" + gameObject.name + "\"");

        var UI = gameObject.GetComponent<T>();
        if (UI != null)
            UI.enabled = false;
        
        if (_affectChilds)
            foreach (var cUI in GetComponentsInChildren<T>())
                cUI.enabled = false;
    }

    private void Show<T>() where T : MonoBehaviour
    {
        //Debug.Log("Mostrando la UI: \"" + gameObject.name + "\"");

        var UI = gameObject.GetComponent<T>();
        if (UI != null)
            UI.enabled = true;

        if (_affectChilds)
            foreach (var cUI in GetComponentsInChildren<T>())
                cUI.enabled = true;
    }

    public void Hide()
    {
        if (_label)
            Hide<UILabel>();
        if (_sprite)
            Hide<UISprite>();
        if (_uiTextures)
            Hide<UITexture>();
    }

    public void Show()
    {
        if (_label)
            Show<UILabel>();
        if (_sprite)
            Show<UISprite>();
        if (_uiTextures)
            Show<UITexture>();
    }

	#endregion
}

