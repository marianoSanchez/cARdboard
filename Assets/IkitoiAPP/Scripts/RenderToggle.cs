using UnityEngine;
using System.Collections;

public class RenderToggle : MonoBehaviour
{
	#region Variables (private)
    
	[HideInInspector]
	[SerializeField]
	private bool _showing;
	[SerializeField]
	private bool _affectChilds;

	#endregion

	#region Propiedades (public)
		
	[ExposeProperty]
	public bool Showing
	{
		get
		{
			return _showing;
		}
		set
		{
			_showing = value;
#if !UNITY_EDITOR
			if (_showing)
				Show();
			else
				Hide();
#endif
		}
	}

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
		this.Showing = _showing;
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	
	}

	#endregion

	#region Metodos

    public void Hide()
    {
        Debug.Log("Ocultando el objeto: \"" + gameObject.name + "\"");
        var rend = gameObject.GetComponent<Renderer>();
        if (rend)
            rend.enabled = false;

		if (_affectChilds)
			foreach (var cRend in GetComponentsInChildren<Renderer>())
				cRend.enabled = false;
    }

    public void Show()
    {
        Debug.Log("Mostrando el objeto: \"" + gameObject.name + "\"");
        var rend = gameObject.GetComponent<Renderer>();
        if (rend)
			rend.enabled = true;

		if (_affectChilds)
			foreach (var cRend in GetComponentsInChildren<Renderer>())
				cRend.enabled = true;
    }

	#endregion
}

