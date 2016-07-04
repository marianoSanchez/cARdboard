using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HWNonMenuExit : MonoBehaviour
{
	#region Variables (private)
    
	#endregion

	#region Propiedades (public)

    public List<EventDelegate> onClick = new List<EventDelegate>();

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EventDelegate.IsValid(onClick))
                EventDelegate.Execute(onClick);

            IkitoiAPP.Instance.GoToMenu();
        }
	}

	#endregion

	#region Metodos

	#endregion
}

