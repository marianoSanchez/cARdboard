using UnityEngine;
using System.Collections;

public class URLOpener : MonoBehaviour
{
	#region Variables (private)
    
	#endregion

	#region Propiedades (public)

    public string URL = "http://tienda.ikitoi.com/";

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
	
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	
	}
    
	#endregion

	#region Metodos

    public void OpenURL()
    {
        Application.OpenURL(URL);
    }

	#endregion
}

