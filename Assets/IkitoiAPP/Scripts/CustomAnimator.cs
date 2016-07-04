using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[RequireComponent(typeof(Animation))]
public class CustomAnimator : MonoBehaviour
{

	#region Variables (private)

    private Animation _animation;
    private ActiveAnimation _activeAnimation;
	
	private int _animIndex = 0;
    private float _animationSpeed = 1;

    private State _state = State.Stopped;
    private State _prePauseState = State.Stopped;

	#endregion

	#region Propiedades (public)

	[HideInInspector]
	public List<EventDelegate> onForward = new List<EventDelegate>();
	[HideInInspector]
	public List<EventDelegate> onBackward = new List<EventDelegate>();
	[HideInInspector]
	public List<EventDelegate> onRewind = new List<EventDelegate>();
    [HideInInspector]
	public List<EventDelegate> onSkip = new List<EventDelegate>();
	[HideInInspector]
    public List<EventDelegate> onAnimIndexChange = new List<EventDelegate>();
    [HideInInspector]
    public List<EventDelegate> onFinished = new List<EventDelegate>();

    public bool atomicAnimations = true;
    public bool hasEndingAnimation = true;
    public bool playEnding = true;

    [ExposeProperty]
    public int AnimIndex
    {
        get
        {
            return _animIndex;
        }
        set
        {

        }
    }	

    [ExposeProperty]
    public float AnimationSpeed
    {
        get 
        { 
            return _animationSpeed; 
        }
        set 
        { 
            _animationSpeed = value;
            UpdateSpeed();
        }
	}
	
	public Animation Animation
	{
		get
		{
			return _animation;
		}
	}

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
		if (_animation == null)
			_animation = GetComponent<Animation>();

        if (_activeAnimation == null)
            FindOrCreateActiveAnimationComp();

		if (_animation != null)
			_animation.playAutomatically = false;
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

    private void FindOrCreateActiveAnimationComp()
    {
        _activeAnimation = GetComponent<ActiveAnimation>();
        if (_activeAnimation == null)
            _activeAnimation = gameObject.AddComponent<ActiveAnimation>();
    }

    private bool IsPlaying()
    {
        return _state == State.PlayingForward || _state == State.PlayingBackward;
    }

    private bool IsPaused()
    {
        return _state == State.Paused;
    }

    private bool IsStopped()
    {
        return _state == State.Stopped;
    }

    private bool CheckAtomicity()
    {
        return !(atomicAnimations && IsPlaying());
    }

    public bool AreMoreClipsForward(int offset = 0)
    {
        return (_animIndex + offset) < _animation.GetClipCount() - (hasEndingAnimation ? 1 : 0);
    }

    public bool AreMoreClipsBehind()
    {
        return _animIndex > 0;
    }

    private string CurrentClip(int offset = 0)
    {
        string result;
        var currClip = _animIndex + 1 + offset;

        if (hasEndingAnimation && currClip == _animation.GetClipCount())
            result = "Ending";
        else
            result = currClip.ToString();

        return result;
    }

    private int NonEndingAnimationsCount()
    {
        return _animation.GetClipCount() - (hasEndingAnimation ? 1 : 0);
    }

    private void ResetIndex()
    {
        _animIndex = 0;
        if (EventDelegate.IsValid(onAnimIndexChange))
            EventDelegate.Execute(onAnimIndexChange);
    }

    private void IndexToEnd()
    {
        _animIndex = NonEndingAnimationsCount();
        if (EventDelegate.IsValid(onAnimIndexChange))
            EventDelegate.Execute(onAnimIndexChange);
    }

    private void IncreaseIndex()
    {
		_animIndex++;
		if (EventDelegate.IsValid(onAnimIndexChange))
			EventDelegate.Execute(onAnimIndexChange);
    }

    private void DecreaseIndex()
    {
		_animIndex--;
		if (EventDelegate.IsValid(onAnimIndexChange))
			EventDelegate.Execute(onAnimIndexChange);
    }

    private void SetOnFinishedAction()
    {
        EventDelegate.Add(_activeAnimation.onFinished, () =>
        {
            if (_state == State.PlayingForward) IncreaseIndex();
            _state = State.Stopped;

            if (hasEndingAnimation && !AreMoreClipsForward())
                PlayEnding();

            if (EventDelegate.IsValid(this.onFinished))
                EventDelegate.Execute(this.onFinished); 
        });
    }

    private void UpdateSpeed()
    {
        if (IsPlaying())
        {
            _animation[CurrentClip()].speed = _state == State.PlayingForward ? _animationSpeed : _animationSpeed * -1;
        }
    }

    public void SetSteppedSpeed(int step)
    {
        if (step < 0) step = 0;
        else if (step > 6) step = 6;

        switch (step)
        {
            case 0: AnimationSpeed = .25f; break;
            case 1: AnimationSpeed = .5f; break;
            case 2: AnimationSpeed = .75f; break;
            case 3: AnimationSpeed = 1f; break;
            case 4: AnimationSpeed = 2f; break;
            case 5: AnimationSpeed = 3f; break;
            case 6: AnimationSpeed = 4f; break;
        }
    }
    
    private void PlayEnding()
    {
        if (!hasEndingAnimation || !playEnding) return;

        Debug.Log("Reproduciendo Ending");

        _animation["Ending"].speed = _animationSpeed;
        _animation.PlayQueued("Ending");
        _animation.enabled = true;
    }

    public void PlayForward()
    {
        if (IsPaused())
        {
            _prePauseState = State.PlayingForward;
            TogglePause();
        }

		else if (AreMoreClipsForward() && CheckAtomicity())
        {            
            var currClip = CurrentClip();

            Debug.Log("Reproduciendo animación \"" + currClip + "\"");

            _animation[currClip].speed = _animationSpeed;
            ActiveAnimation.Play(_animation, currClip, AnimationOrTween.Direction.Forward);
            _state = State.PlayingForward;

            SetOnFinishedAction();

			if (EventDelegate.IsValid(onForward))
				EventDelegate.Execute(onForward);
        }
    }

    public void Replay()
    {
        Rewind();
        PlayForward();
    }
    
    public void PlayBackwards()
    {
        if (IsPaused())
        {
            _prePauseState = State.PlayingBackward;
            TogglePause();
        }

        else if (AreMoreClipsBehind() && CheckAtomicity())
        {
            var currClip = CurrentClip(-1);

            Debug.Log("Retrocediendo animación \"" + currClip + "\"");

            _animation[currClip].speed = _animationSpeed * -1;
            ActiveAnimation.Play(_animation, currClip, AnimationOrTween.Direction.Reverse);
            _state = State.PlayingBackward;
            DecreaseIndex();

            SetOnFinishedAction();
			
			if (EventDelegate.IsValid(onBackward))
				EventDelegate.Execute(onBackward);
        }
    }
    
    public void Rewind()
    {
        if (!IsStopped())
        {
            if (CheckAtomicity())
            {
                ActiveAnimation.Play(_animation, AnimationOrTween.Direction.Reverse);
                _activeAnimation.Finish();
                _state = State.Stopped;
            }
        }
        else if (AreMoreClipsBehind())
        {
            var currClip = CurrentClip(-1);

            Debug.Log("Rebobinando animación \"" + currClip + "\"");

            _animation[currClip].speed = _animationSpeed * -1;
            ActiveAnimation.Play(_animation, currClip, AnimationOrTween.Direction.Reverse);
            _activeAnimation.Finish();
            _state = State.Stopped;
            DecreaseIndex();
        }
		
		if (EventDelegate.IsValid(onRewind))
			EventDelegate.Execute(onRewind);
    }

    public void RewindAll()
    {
        if (CheckAtomicity())
        {
            for (int i = NonEndingAnimationsCount(); i > 0; i--)
            {
                ActiveAnimation.Play(_animation, i.ToString(), AnimationOrTween.Direction.Reverse);
                _activeAnimation.Finish();
                _state = State.Stopped;
            }

            ResetIndex();

            if (EventDelegate.IsValid(onRewind))
                EventDelegate.Execute(onRewind);
        }
    }

    public void Skip()
    {
        if (!IsStopped())
        {
            if (CheckAtomicity())
            {
                ActiveAnimation.Play(_animation, AnimationOrTween.Direction.Forward);
                _activeAnimation.Finish();
                _state = State.Stopped;
                IncreaseIndex();
            }
        }
        else if (AreMoreClipsForward())
        {
            PlayForward();
            _activeAnimation.Finish();
        }
		
		if (EventDelegate.IsValid(onSkip))
			EventDelegate.Execute(onSkip);
    }

    public void SkipAll()
    {
        if (CheckAtomicity())
        {
            IndexToEnd();

            for (int i = 1; i <= _animIndex; i++)
            {
                ActiveAnimation.Play(_animation, i.ToString(), AnimationOrTween.Direction.Forward);
                _activeAnimation.Finish();
                _state = State.Stopped;
            }

            if (hasEndingAnimation)
                PlayEnding();

            if (EventDelegate.IsValid(this.onFinished))
                EventDelegate.Execute(this.onFinished); 
        }
    }

    public void TogglePause()
    {
        var currClip = CurrentClip();

        if (IsPlaying())
        {
            _animation[currClip].speed = 0;
            _prePauseState = _state;
            _state = State.Paused;
        }
        else if (IsPaused())
        {
            var resumeSpeed = _prePauseState == State.PlayingForward ? _animationSpeed : _animationSpeed  * -1;
            _animation[currClip].speed = resumeSpeed;
            _state = _prePauseState;
        }
    }

    public enum State
    {
        PlayingForward,
        PlayingBackward,
        Paused,
        Stopped
    }

	#endregion
}
