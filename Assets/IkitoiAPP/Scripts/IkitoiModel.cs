using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[RequireComponent(typeof(AnimationsDescription))]
[RequireComponent(typeof(CustomAnimator))]
[RequireComponent(typeof(ModelParts))]
public class IkitoiModel : MonoBehaviour
{
	#region Variables (private)
    
    [HideInInspector]
    [SerializeField]
    UIAtlas _atlas;

    [HideInInspector]
    [SerializeField]
    string _spriteName;
    
	#endregion

	#region Propiedades (public)

	public UIAtlas atlas
	{
		get
		{
			return _atlas;
		}
	}

	public string spriteName
	{
		get
		{
			return _spriteName;
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

