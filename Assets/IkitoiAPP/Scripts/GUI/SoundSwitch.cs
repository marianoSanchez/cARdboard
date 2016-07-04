using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ext_Switch))]
public class SoundSwitch : MonoBehaviour
{
	#region Variables (private)

    private ext_Switch _switch;
    
	#endregion

	#region Propiedades (public)
    
	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (_switch == null)
            _switch = GetComponent<ext_Switch>();        
    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	void Start ()	
	{
        CheckStatus();
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	
	}

	#endregion

	#region Metodos

	public void CheckStatus()
	{
        bool soundEnabled = false;
        if (NGUITools.soundVolume == 0f)
		{
            soundEnabled = false;
		}
		else
        {
            soundEnabled = true;
		}

        _switch.State = soundEnabled;
	}

	#endregion
}