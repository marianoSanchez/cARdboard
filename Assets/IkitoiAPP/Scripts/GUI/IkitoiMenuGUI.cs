using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IkitoiMenuGUI : MonoBehaviour
{
	#region Variables (private)    

    private static IkitoiMenuGUI _instance;

    private UIPlayTween _tweenPlayer;

    [SerializeField]
	private int _inTransition = 0;
    [SerializeField]
    private MenuState _menuState = MenuState.Main;
	
    [Header("Paneles UI")]
    [SerializeField]
    private UIPanel _background;
	[SerializeField]
	private UIPanel _backgroundBtns;
    [SerializeField]
    private UIPanel _main;
    [SerializeField]
    private UIPanel _printMarker;
    [SerializeField]
    private UIPanel _models;

    [Header("PopUps")]
    [SerializeField]
    private UIPopUp _help;
    [SerializeField]
    private UIPopUp _settings;
    [SerializeField]
    private UIPopUp _parts;


	#endregion

    #region Propiedades (public)

    public static IkitoiMenuGUI Instance
    {
        get { return IkitoiMenuGUI._instance; }
    }

    [Space(15)]
    public float transitionDuration = .3f;
    
    public MenuState UIState
    {
        get { return _menuState; }
    }

    public UIPanel Main
    {
        get { return _main; }
    }

    public UIPanel PrintMarker
    {
        get { return _printMarker; }
    }

    public UIPanel Models
    {
        get { return _models; }
    }

    public UIPopUp Help
    {
        get { return _help; }
    }

    public UIPopUp Parts
    {
        get { return _parts; }
    }

    public UIPopUp Settings
    {
        get { return _settings; }
    }
    
	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (IkitoiMenuGUI._instance == null)
            IkitoiMenuGUI._instance = this;

        if (_tweenPlayer == null)
            _tweenPlayer = FindOrCreateTweenPlayer(this);
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
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            GoBack(); 
        }
	}

	#endregion

	#region Metodos
        
    public bool IsOnTransition()
    {
        return _inTransition > 0;
    }
    
    #region Menus

    private void GoToState(MenuState newState, UIPanel[] hide, UIPanel[] show)
    {
        // Si esta en una transicion vuelve
        if (IsOnTransition()) return;

        // Si hay un popUp no cambia de menu
        //if (IsOnPopUp()) return;

        //
        foreach (var panel in hide)
        {
            HideUI(panel, transitionDuration);
        }

        //
        foreach(var panel in show)
        {
            ShowUI(panel, transitionDuration);
        }

        _menuState = newState;
    }
    	
	public void GoToMain()
    {
        // Desde donde?
        switch (_menuState)
        {
            case MenuState.PrintMarker:
                GoToState(MenuState.Main,
                    new UIPanel[] { _printMarker },
                    new UIPanel[] { _backgroundBtns, _main });
                break;

            case MenuState.Models:
                GoToState(MenuState.Main,
                    new UIPanel[] { _models },
                    new UIPanel[] { _backgroundBtns, _main });
                break;
        }
	}
	
    public void GoToModels()
    {
        // Desde donde?
        switch (_menuState)
        {
            case MenuState.Main:
                GoToState(MenuState.Models,
                    new UIPanel[] { _main, _backgroundBtns },
                    new UIPanel[] { _models });
                break;

            case MenuState.PrintMarker:
                GoToState(MenuState.Models,
                    new UIPanel[] { _printMarker },
                    new UIPanel[] { _models });
                break;
        }
	}
	
	public void GoToPrintMarker()
    {
        // Desde donde?
        switch (_menuState)
        {
            case MenuState.Main:
                GoToState(MenuState.PrintMarker,
                    new UIPanel[] { _main, _backgroundBtns },
                    new UIPanel[] { _printMarker });
                break;
        }
	}

    #endregion

    public void GoBack()
    {
        // Si esta en transicion ignora
        if (IsOnTransition()) return;

        // En que menu esta?
        else
        {
            switch (_menuState)
            {
                case MenuState.Main:
                    IkitoiAPP.Instance.Quit();
                    break;

                case MenuState.PrintMarker:
                    GoToMain();
                    break;

                case MenuState.Models:
                    GoToMain();
                    break;
            }
        }
    }

    private void HideUI(UIPanel panel, float duration)
    {
        _inTransition++;

        UITweener tweener;
        tweener = FindOrCreateTweener<TweenAlpha>(panel);
        tweener.duration = duration;

		_tweenPlayer.tweenTarget = tweener.gameObject;
        _tweenPlayer.ifDisabledOnPlay = AnimationOrTween.EnableCondition.EnableThenPlay;
        EventDelegate.Add(_tweenPlayer.onFinished, () => { _inTransition--;  tweener.gameObject.SetActive(false); }, true);
        _tweenPlayer.Play(false);
    }

	private void ShowUI(UIPanel panel, float duration)
    {		
        _inTransition++;

		UITweener tweener;
        tweener = FindOrCreateTweener<TweenAlpha>(panel);
        tweener.duration = duration;
        
        _tweenPlayer.tweenTarget = tweener.gameObject;
        _tweenPlayer.ifDisabledOnPlay = AnimationOrTween.EnableCondition.EnableThenPlay;
        EventDelegate.Add(tweener.onFinished, () => _inTransition--, true);
        _tweenPlayer.Play(true);
    }

    private UITweener FindOrCreateTweener<T>(UIPanel panel) where T : UITweener
    {
        UITweener tweener = panel.GetComponent<T>();
        if (!tweener)
        {
            tweener = panel.gameObject.AddComponent<T>();

            if (typeof(T) == typeof(TweenAlpha))
                SetAlphaTweener(ref tweener);

            else if (typeof(T) == typeof(TweenScale))
                SetScaleTweener(ref tweener);
        }

        return tweener;
    }

    private UIPlayTween FindOrCreateTweenPlayer(MonoBehaviour mBehav)
    {
        var player = mBehav.GetComponent<UIPlayTween>();
        if (!player)
            player = mBehav.gameObject.AddComponent<UIPlayTween>();

		player.trigger = AnimationOrTween.Trigger.OnActivate;

        return player;
    }

    private void SetAlphaTweener(ref UITweener tweener)
    {
        (tweener as TweenAlpha).from = 0;
    }

    private void SetScaleTweener(ref UITweener tweener)
    {
        (tweener as TweenScale).from = Vector3.zero;
	}

	#endregion

    public enum MenuState
    {
        Loading,
        Main,
        PrintMarker,
        Models,
		LoadingCam
    }
}

