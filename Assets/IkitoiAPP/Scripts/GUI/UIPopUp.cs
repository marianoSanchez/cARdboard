using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIPopUp : MonoBehaviour
{
    public enum OutsideClickBehaviour
    {
        DoNothing,
        Block,
        Close
    }

	#region Variables (private)

    protected const float safeClickTime = .2f;

    [SerializeField]
    protected Transform _window;
    [SerializeField]
    protected Transform _background;

    protected bool _inTransition;
    protected float _timerClick = safeClickTime;

    protected UICustomPlayTween _tweenPlayer;
    protected virtual UICustomPlayTween TweenPlayer
    {
        get
        {
            if (_tweenPlayer == null)
                _tweenPlayer = FindOrCreateTweenPlayer(gameObject);

            return _tweenPlayer;
        }
    }
    
    [SerializeField]
    protected UIButton _cancelBtn;
    [SerializeField]
    protected UIButton _acceptBtn;

    [SerializeField]
    protected bool _darkenBackground;
    [Range(0, 1)]
    [SerializeField]
    protected float _darkenAlpha = .7f;

    [SerializeField]
    protected OutsideClickBehaviour _outBehave = OutsideClickBehaviour.DoNothing;
    protected BoxCollider _backgroundCollider;

	#endregion

	#region Propiedades (public)

    public float transitionDuration = .2f;

    [Space(10)]
    public List<EventDelegate> onCancel = new List<EventDelegate>();
    public List<EventDelegate> onAccept = new List<EventDelegate>();
                    
    public virtual OutsideClickBehaviour OutBehave
    {
        get 
        { 
            return _outBehave; 
        }
        set 
        {
            if (_outBehave != value)
            {
                _outBehave = value;
                UpdateOutBehave();
            }
        }
    }

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    protected virtual void Awake()
    {
        if (_acceptBtn != null)
            EventDelegate.Add(_acceptBtn.onClick, Accept);

        if (_cancelBtn != null)
            EventDelegate.Add(_cancelBtn.onClick, Cancel);

        if (_background == null)
        {
            _background = new GameObject().transform;
            _background.gameObject.name = "Background";
            _background.parent = this.transform;
            _background.localScale = Vector3.one;
        }

        var darkTexture = _background.GetComponent<UITexture>();
        if (_darkenBackground)
        {
            if (darkTexture == null)
                CreateDarkTexture(_background.gameObject);
            else
                darkTexture.enabled = true;
        }
        else if (darkTexture != null)
            darkTexture.enabled = false;

        _backgroundCollider = NGUITools.AddMissingComponent<BoxCollider>(_background.gameObject);
        var screenSize = ScreenSize();
        _backgroundCollider.size = new Vector3(screenSize.x, screenSize.y, 1);
        UpdateOutBehave();
    }

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	protected virtual void Update () 
	{
        if (_outBehave == OutsideClickBehaviour.Close)
        {
            if (CheckOutsideClick())
                Close();
        }
	}

    protected virtual void OnEnable()
    {
        _timerClick = safeClickTime;
    }

	#endregion

	#region Metodos
    
    public virtual void Show()
    {
        if (!_inTransition)
            ShowUI(transitionDuration);
    }

    public virtual void Close()
    {
        if (!_inTransition)
            CloseUI(transitionDuration);
    }

    public virtual void Accept()
    {
        if (EventDelegate.IsValid(onAccept))
            EventDelegate.Execute(onAccept);

        Close();
    }

    public virtual void Cancel()
    {
        if (EventDelegate.IsValid(onCancel))
            EventDelegate.Execute(onCancel);

        Close();
    }

    protected virtual void CloseUI(float duration)
    {
        _inTransition = true;
        gameObject.SetActive(true);

        UITweener tweener;
        tweener = FindOrCreateTweener<TweenAlpha>(_background.gameObject);
        tweener.duration = duration;
        tweener = FindOrCreateTweener<TweenScale>(_window.gameObject);
        tweener.duration = duration;

        EventDelegate.Add(TweenPlayer.onFinished, () => { _inTransition = false; gameObject.SetActive(false); }, true);
        _tweenPlayer.Play(false);
    }

    protected virtual void ShowUI(float duration)
    {
        _inTransition = true;
        gameObject.SetActive(true);

        UITweener tweener;
        tweener = FindOrCreateTweener<TweenAlpha>(_background.gameObject);
        tweener.duration = duration;
        tweener = FindOrCreateTweener<TweenScale>(_window.gameObject);
        tweener.duration = duration;

        EventDelegate.Add(TweenPlayer.onFinished, () => _inTransition = false, true);
        TweenPlayer.Play(true);
    }

    protected virtual UITexture CreateDarkTexture(GameObject container)
    {
        var blackTexture = new Texture2D(2,2);
        blackTexture.SetPixels(0, 0, 2, 2, Enumerable.Repeat<Color>(Color.black, 4).ToArray());
        blackTexture.Apply();

        var screenSize = ScreenSize();
        var uiTexture = container.AddComponent<UITexture>();
        uiTexture.mainTexture = blackTexture;
        uiTexture.color = Color.black;
        uiTexture.width = (int)(screenSize.x + 2);
        uiTexture.height = (int)(screenSize.y + 2);
        uiTexture.depth = -10;

        return uiTexture;
    }

    protected virtual UICustomPlayTween FindOrCreateTweenPlayer(GameObject gObject)
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

    protected virtual UITweener FindOrCreateTweener<T>(GameObject gObject) where T : UITweener
    {
        UITweener tweener = gObject.GetComponent<T>();
        if (!tweener)
        {
            tweener = gObject.gameObject.AddComponent<T>();

            if (typeof(T) == typeof(TweenAlpha))
                SetAlphaTweener(ref tweener);

            else if (typeof(T) == typeof(TweenScale))
                SetScaleTweener(ref tweener);
        }

        return tweener;
    }

    protected virtual void SetAlphaTweener(ref UITweener tweener)
    {
        (tweener as TweenAlpha).from = 0;
        (tweener as TweenAlpha).to = _darkenAlpha;
    }

    protected virtual void SetScaleTweener(ref UITweener tweener)
    {
        (tweener as TweenScale).from = Vector3.zero;
    }

    protected virtual void UpdateOutBehave()
    {
        if (_outBehave == OutsideClickBehaviour.DoNothing ||
            _outBehave == OutsideClickBehaviour.Close)
        {
            _backgroundCollider.enabled = false;
            _timerClick = safeClickTime;
        }
        else if (_outBehave == OutsideClickBehaviour.Block)
        {
            _backgroundCollider.enabled = true;
        }
    }

    protected virtual bool CheckOutsideClick()
    {
        bool outClick = false;

        // While the delay timer is active, this component won't be able to auto-disable the UI. (See note above on the timer.)
        if (_timerClick > 0f)
        {
            // Decrease timer
            _timerClick -= Time.deltaTime;
        }
        else if (UICamera.CountInputSources() == 0)
        {
            // No tap this frame
        }
        else
        {
            // Get tapped or selected object
            var gObject = UICamera.hoveredObject;
            if (gObject == null)
                gObject = UICamera.selectedObject;

            // Selected UI element,
            // Tap this frame, and it was not on any UI. Outside click!
            // Tap this frame, and it was not on this object or any of its children. Outside click!
            if (gObject != null && !gObject.transform.IsChildOf(this.transform))
            {
                outClick = true;
            }
        }

        return outClick;
    }

    protected Vector2 ScreenSize()
    {
        var UIRoot = NGUITools.GetRoot(this.gameObject).GetComponent<UIRoot>();
        float ratio = (float)UIRoot.activeHeight / Screen.height;

        return new Vector2(Mathf.Ceil(Screen.width * ratio), Mathf.Ceil(Screen.height * ratio));
    }

	#endregion
}

