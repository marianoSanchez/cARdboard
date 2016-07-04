using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnTakeScreenshot : MonoBehaviour
{
	#region Variables (private)
        
	#endregion

	#region Propiedades (public)

    public List<UIRect> _panelsToHide = new List<UIRect>();
    public UIPhotoPopUp _photoPopUp;

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
        if (_photoPopUp == null || _photoPopUp.isActiveAndEnabled) return;

        if (_panelsToHide.Count > 0)
        {
            foreach (var panel in _panelsToHide)
                panel.alpha = 0f;
        }

        StartCoroutine(TakeScreenshotAndShow());
	}

	#endregion

	#region Metodos

    IEnumerator TakeScreenshotAndShow()
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        if (_panelsToHide.Count > 0)
        {
            foreach (var panel in _panelsToHide)
                panel.alpha = 1f;
        }

        _photoPopUp.SetPhotoTexture(tex);
        _photoPopUp.Show();
    }

	#endregion
}

