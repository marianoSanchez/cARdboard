using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ModelParts : MonoBehaviour
{
	#region Variables (private)

    public int[] _parts = new int[0];

	#endregion

	#region Propiedades (public)

    public ReadOnlyCollection<int> Parts
    {
        get
        {
            return new ReadOnlyCollection<int>(_parts);
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
	
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	
	}

	#endregion

	#region Metodos

	#endregion
}

