using UnityEngine;
using System;
using System.Collections;

public class ModelLoader : MonoBehaviour
{
	#region Variables (private)

    [SerializeField]
    private GameObject _modelsContainer;

    [Header("GUI")]
    [SerializeField]
    private UIButton _btnReplay;
	[SerializeField]
	private UIButton _btnForward;
	[SerializeField]
    private UIButton _btnBackward;
	[SerializeField]
    private UIButton _btnRewind;
    [SerializeField]
    private UIButton _btnRewindAll;
    [SerializeField]
    private UIButton _btnSkip;
    [SerializeField]
    private UIButton _btnSkipAll;
    [SerializeField]
	private UIButton _btnPause;
    [SerializeField]
    private UILabel _stepIndicator;
    [SerializeField]
    private UISlider _speedSlider;
    [SerializeField]
    private UILabel _speedIndicator;
    [SerializeField]
    private UILabel _descriptionLabel;
    [Space(10)]
    [SerializeField]
    private UIPopUp _photoPopUp;
    [Header("Sounds")]
    [SerializeField]
    private AudioClip _forwardClip;
    [SerializeField]
    private AudioClip _backwardClip;

    int counter = 0;
    float timer = 0f;
    float timeLastPressed = 0f;

	#endregion

	#region Propiedades (public)

    public int PressesUntilPopUp = 2;
    public float TimerUntilPopUp = 2f;

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
        if (IkitoiAPP.SelectedModel != null)
            LoadModel(IkitoiAPP.SelectedModel);
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	
	}

	#endregion

	#region Metodos

    public void LoadModel(GameObject model)
    {
        var iModel = Instantiate<GameObject>(model);
		var prevScale = iModel.transform.localScale;
        iModel.transform.parent = _modelsContainer.transform;
        iModel.transform.localScale = prevScale;

        // Fuerza el estado inicial del model
        var animator = iModel.GetComponent<CustomAnimator>();
        animator.SkipAll();
        animator.RewindAll();

        SetUIReference(iModel);
    }

    private void SetUIReference(GameObject instantiatedModel)
    {
        var mAnimator = instantiatedModel.GetComponent<CustomAnimator>();

        // Sounds

        // Forward
        if (_forwardClip != null)
        {
            EventDelegate.Add(mAnimator.onForward, () =>
            {
                NGUITools.PlaySound(_forwardClip);
            });
        }

        // Backward
        if (_backwardClip != null)
        {
            EventDelegate.Add(mAnimator.onBackward, () =>
            {
                NGUITools.PlaySound(_backwardClip);
            });
        }

        // Step Indicator
        if (_stepIndicator)
        {
            EventDelegate.Callback action = () =>
            {
                _stepIndicator.text = mAnimator.AnimIndex.ToString();
            };
            EventDelegate.Add(mAnimator.onAnimIndexChange, action);

            action();
        }

        // Speed Slider
        if (_speedIndicator)
        {
            EventDelegate.Add(_speedSlider.onChange, () =>
            {
                var currStep = Mathf.Round(_speedSlider.value * (_speedSlider.numberOfSteps - 1));
                mAnimator.SetSteppedSpeed((int)currStep);
            });

            EventDelegate.Add(_speedSlider.onChange, () => 
            {
                _speedIndicator.text = mAnimator.AnimationSpeed.ToString() + "x"; 
            });
        }

        // Description Label
        if (_descriptionLabel)
        {
            EventDelegate.Callback action = () =>
            {
                var descriptionsComp = IkitoiAPP.SelectedModel.GetComponent<AnimationsDescription>();

                var text = string.Empty;
                if (descriptionsComp.descArray.Length >= mAnimator.AnimIndex)
                    text = descriptionsComp.descArray[mAnimator.AnimIndex];

                _descriptionLabel.text = text;
            };
            EventDelegate.Add(mAnimator.onAnimIndexChange, action);

            action();
        }

        // Btn Replay
        if (_btnReplay)
        {
            EventDelegate.Add(_btnReplay.onClick, () => mAnimator.Replay());
        }

        // Btn Forward
        if (_btnForward)
        {
            EventDelegate.Add(_btnForward.onClick, () => mAnimator.PlayForward());
            if (_photoPopUp)
            {
                EventDelegate.Add(_btnForward.onClick, () =>
                {         
                    if (!mAnimator.AreMoreClipsForward() && !_photoPopUp.isActiveAndEnabled)
                    {
                        if (++counter >= PressesUntilPopUp && timer >= TimerUntilPopUp)
                        {
                            _photoPopUp.Show();
                        }
                        else
                        {
                            timer += Time.realtimeSinceStartup - timeLastPressed;
                            timeLastPressed = Time.realtimeSinceStartup;
                        }
                    }

                    else if (!mAnimator.AreMoreClipsForward(1))
                    {
                        timeLastPressed = Time.realtimeSinceStartup;
                    }
                });
            }
        }

        // Btn Backward
        if (_btnBackward)
        {
            EventDelegate.Add(_btnBackward.onClick, () => 
            {
                mAnimator.PlayBackwards();
                timeLastPressed = 0f;
                timer = 0f;
                counter = 0;
            });
        }

        // Btn Rewind
        if (_btnRewind)
        {
            EventDelegate.Add(_btnRewind.onClick, () => mAnimator.Rewind());
        }

        // Btn Rewind
        if (_btnRewindAll)
        {
            EventDelegate.Add(_btnRewindAll.onClick, () => mAnimator.RewindAll());
        }


        // Btn Skip
        if (_btnSkip)
        {
            EventDelegate.Add(_btnSkip.onClick, () => mAnimator.Skip());
        }


        // Btn Rewind
        if (_btnSkipAll)
        {
            EventDelegate.Add(_btnSkipAll.onClick, () => mAnimator.SkipAll());
        }

        // Btn Pause
        if (_btnPause)
        {
            EventDelegate.Add(_btnPause.onClick, () => mAnimator.TogglePause());
        }
    }

	#endregion
}

