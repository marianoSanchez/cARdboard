using UnityEngine;
using System.Collections;

public class ActivationToggle : MonoBehaviour
{
	#region Variables (private)
	
	#endregion

	#region Propiedades (public)
	
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

    public void Disable()
    {
        Debug.Log("Deshabilitando el objeto: \"" + gameObject.name + "\"");
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        Debug.Log("Habilitando el objeto: \"" + gameObject.name + "\"");
        gameObject.SetActive(true);
    }

    public void Toggle()
    {
        if (gameObject.activeInHierarchy)
            Disable();
        else
            Enable();
    }

	#endregion
}

