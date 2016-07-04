using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animation))]
public class AnimationsDescription : MonoBehaviour
{
	#region Variables (private)

    private Animation _anim;
        
	#endregion

	#region Propiedades (public)

    public string[] descArray = new string[0];

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (_anim == null)
            _anim = GetComponent<Animation>();
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

