using UnityEngine;
using System.Collections;

public class AudioToggle : MonoBehaviour
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

    public void ToggleAudio()
    {
        NGUITools.soundVolume = 1f - NGUITools.soundVolume;
        Debug.Log("Sonido: " + (NGUITools.soundVolume == 1f ? "On" : "Off"));
    }

    public void SetAudioOn()
    {
        SetAudio(true);
    }

    public void SetAudioOff()
    {
        SetAudio(false);
    }

    public void SetAudio(bool enable)
    {
        NGUITools.soundVolume = enable ? 1f : 0f;
        Debug.Log("Sonido: " + (enable ? "On" : "Off"));
    }

	#endregion
}

