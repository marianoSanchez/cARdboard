using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ModelsIndex : MonoBehaviour
{
	#region Variables (private)

    // Singleton pattern
    private static ModelsIndex _instance;

    // Variables de instancia
    [SerializeField]
    private List<IkitoiModel> _models = new List<IkitoiModel>();

	#endregion

	#region Propiedades (public)

    public static ModelsIndex instance
    {
        get
        {
            return _instance;
        }
    }

    public static List<IkitoiModel> Models
    {
        get
        {
            return _instance._models;
        }
    }

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (ModelsIndex._instance == null)
            ModelsIndex._instance = this;
    }

	#endregion

	#region Metodos
        
	#endregion
}
