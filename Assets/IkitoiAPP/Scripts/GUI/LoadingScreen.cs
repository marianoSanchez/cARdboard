using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
	#region Variables (private)

    private bool _inTransition;
    private UICustomPlayTween _tweenPlayer;
    private UICustomPlayTween TweenPlayer
    {
        get
        {
            if (_tweenPlayer == null)
                _tweenPlayer = FindOrCreateTweenPlayer(gameObject);

            return _tweenPlayer;
        }
    }

	#endregion

	#region Propiedades (public)

    public float transitionDuration = .2f;

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

    public void Show()
    {
        if (!_inTransition)
            ShowUI(transitionDuration);
    }

    public void Close()
    {
        if (!_inTransition)
            CloseUI(transitionDuration);
    }

    private void CloseUI(float duration)
    {
        _inTransition = true;
        gameObject.SetActive(true);

        UITweener tweener;
        tweener = FindOrCreateAlphaTweener(gameObject);
        tweener.duration = duration;

        EventDelegate.Add(TweenPlayer.onFinished, () => { _inTransition = false; gameObject.SetActive(false); }, true);
        _tweenPlayer.Play(false);
    }

    private void ShowUI(float duration)
    {
        _inTransition = true;
        gameObject.SetActive(true);

        UITweener tweener;
        tweener = FindOrCreateAlphaTweener(gameObject);
        tweener.duration = duration;

        EventDelegate.Add(TweenPlayer.onFinished, () => _inTransition = false, true);
        TweenPlayer.Play(true);
    }

    public static LoadingScreen Create(Transform parent)
    {
        var prefab = Resources.Load("Prefabs/GUI/LoadingScreen") as GameObject;

        var instance = Instantiate(prefab);
        instance.transform.parent = parent;
        instance.transform.localScale = Vector3.one;
        instance.SetActive(false);

        return instance.GetComponent<LoadingScreen>();
    }

    private UICustomPlayTween FindOrCreateTweenPlayer(GameObject gObject)
    {
        var player = gObject.GetComponent<UICustomPlayTween>();
        if (!player)
            player = gObject.AddComponent<UICustomPlayTween>();

        player.trigger = AnimationOrTween.Trigger.OnActivate;
        player.tweenTarget = gameObject;
        player.includeChildren = true;
        player.ifDisabledOnPlay = AnimationOrTween.EnableCondition.EnableThenPlay;

        return player;
    }

    protected TweenAlpha FindOrCreateAlphaTweener(GameObject gObject)
    {
        var tweener = gObject.GetComponent<TweenAlpha>();
        if (!tweener)
        {
            tweener = gObject.gameObject.AddComponent<TweenAlpha>();
            tweener.from = 0;
            tweener.to = 1;
        }

        return tweener;
    }

	#endregion
}

