using UnityEngine;
using System.Collections;

public class BtnNonMenuExit : MonoBehaviour
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

    void OnClick()
    {
        IkitoiAPP.Instance.GoToMenu();
    }

	#endregion

	#region Metodos

	#endregion
}

