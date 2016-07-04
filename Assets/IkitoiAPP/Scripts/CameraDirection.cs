using UnityEngine;
using System.Collections;
using Vuforia;

public class CameraDirection : MonoBehaviour
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

    public void ToggleDirection()
    {
        var currDir = CameraDevice.Instance.GetCameraDirection();
        if (currDir == CameraDevice.CameraDirection.CAMERA_BACK)
            SetDirection(CameraDevice.CameraDirection.CAMERA_FRONT);
        else
            SetDirection(CameraDevice.CameraDirection.CAMERA_BACK);
    }

    void SetDirection(CameraDevice.CameraDirection direction)
    {
        CameraDevice.Instance.Stop();
        CameraDevice.Instance.Deinit();
        CameraDevice.Instance.Init(direction);
        CameraDevice.Instance.Start();
    }

	#endregion
}

