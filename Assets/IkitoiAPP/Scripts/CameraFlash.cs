using UnityEngine;
using System.Collections;
using Vuforia;

public class CameraFlash : MonoBehaviour
{
	#region Variables (private)

    bool _flashEnabled = false;

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

    public void ToggleFlash()
    {
#if UNITY_IPHONE
        ToggleFlashlight();
#elif UNITY_ANDROID
        ToggleFlashlight();
#endif
    }
    
    void ToggleFlashlight()
    {
        _flashEnabled = !_flashEnabled;
        CameraDevice.Instance.SetFlashTorchMode(_flashEnabled);
    }

    void SetFlashlightOn()
    {
        _flashEnabled = true;
        CameraDevice.Instance.SetFlashTorchMode(true);
    }

    void SetFlashlightOff()
    {
        _flashEnabled = false;
        CameraDevice.Instance.SetFlashTorchMode(false);
    }

	#endregion
}

