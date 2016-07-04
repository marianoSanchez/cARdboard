using UnityEngine;
using System.Collections;

public class IkitoiAPP : MonoBehaviour
{
    #region Variables (private)

    private bool isLoading;

    private bool SPTwitterInited;
    private bool SPFacebookInited;

    // Singleton pattern
	private static IkitoiAPP _instance;

    #endregion

    #region Propiedades (public)
    	
    public static IkitoiAPP Instance
    {
        get
        {
            return _instance;
        }
    }

	public GameObject selectedModel;
	public static GameObject SelectedModel
	{
		get
		{
			return _instance.selectedModel;
		}
		set
		{
			_instance.selectedModel = value;
		}
	}

    #endregion

    #region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (IkitoiAPP._instance == null)
            IkitoiAPP._instance = this;
	}

    /// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
    /// </summary>
    void Start()
    {
        StartCoroutine(InitSocialAPI());
    }
	
	/// <summary>
	/// Carga el nivel
	/// </summary>
	public void OnLevelWasLoaded(int level) 
	{

	}

    #endregion

    #region Metodos

    /// <summary>
    /// Sale de la aplicacion
    /// </summary>
    public void Quit()
    {
        Application.Quit();
	}

	/// <summary>
	/// Carga el nivel de fotos
	/// </summary>
    public void GoToPhoto(float delay = 0f)
	{
        Debug.Log("Cargando escenario de Foto..");
		StartCoroutine(loadAsync(3, delay));
	}

    /// <summary>
    /// Carga el nivel de tutorial
    /// </summary>
    public void GoToTutorial(float delay = 0f)
    {
        Debug.Log("Cargando Tutorial..");
        StartCoroutine(loadAsync(2, delay));
    }

	/// <summary>
	/// Carga el nivel de tutorial
	/// </summary>
	public void GoToMenu(float delay = 0f)
    {
        Debug.Log("Cargando Menu..");
        StartCoroutine(loadAsync(1, delay));
	}

    private IEnumerator loadAsync(int levelIndex, float delay)
    {
        isLoading = true;

        yield return new WaitForSeconds(delay);

        Application.backgroundLoadingPriority = ThreadPriority.Low;
        var async = Application.LoadLevelAsync(levelIndex);
        async.allowSceneActivation = false;

        while (!async.isDone && async.progress < .9f)
            yield return null;

        yield return new WaitForSeconds(.5f);
        async.allowSceneActivation = true;

        isLoading = false;
    }

    IEnumerator InitSocialAPI()
    {
        var socialAPI = SocialAPI.Instance;

        yield return StartCoroutine(socialAPI.InitializeAPIs(true, true, true, false));
        
        if (!Application.isLoadingLevel && !isLoading && Application.loadedLevel != 1)
            GoToMenu();
    }

    #endregion
}

