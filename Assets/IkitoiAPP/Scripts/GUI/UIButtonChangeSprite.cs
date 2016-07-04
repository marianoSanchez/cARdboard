using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AnimationOrTween;

[System.Serializable]
public class UIButtonSprites
{
    [SerializeField]
    /// <summary>
    /// Name of the normal state sprite.
    /// </summary>
    public string normalSprite;

    [SerializeField]
    /// <summary>
    /// Name of the hover state sprite.
    /// </summary>
    public string hoverSprite;

    [SerializeField]
    /// <summary>
    /// Name of the pressed sprite.
    /// </summary>
    public string pressedSprite;

    [SerializeField]
    /// <summary>
    /// Name of the disabled sprite.
    /// </summary>
    public string disabledSprite;
}

[System.Serializable]
public class UIButtonChangeSprite : MonoBehaviour
{
	#region Variables (private)    

    private int _spritesIndex = 0;

	#endregion

	#region Propiedades (public)
    
    public UIButton buttonTarget;

    public Trigger trigger = Trigger.OnClick;

    [HideInInspector]
    [SerializeField]
    public List<UIButtonSprites> sprites = new List<UIButtonSprites>(){ new UIButtonSprites() };

    bool dualState { get { return trigger == Trigger.OnPress || trigger == Trigger.OnHover; } }

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

    void OnHover(bool isOver)
    {
        if (!enabled) return;
        if (trigger == Trigger.OnHover ||
            (trigger == Trigger.OnHoverTrue && isOver) ||
            (trigger == Trigger.OnHoverFalse && !isOver))
            ChangeToNextSprites();
    }

    void OnPress(bool isPressed)
    {
        if (!enabled) return;
        if (UICamera.currentTouchID < -1 && UICamera.currentScheme != UICamera.ControlScheme.Controller) return;
        if (trigger == Trigger.OnPress ||
            (trigger == Trigger.OnPressTrue && isPressed) ||
            (trigger == Trigger.OnPressFalse && !isPressed))
            ChangeToNextSprites();
    }

    void OnClick()
    {
        if (UICamera.currentTouchID < -1 && UICamera.currentScheme != UICamera.ControlScheme.Controller) return;
        if (enabled && trigger == Trigger.OnClick)
            ChangeToNextSprites();
    }

    void OnDoubleClick()
    {
        if (UICamera.currentTouchID < -1 && UICamera.currentScheme != UICamera.ControlScheme.Controller) return;
        if (enabled && trigger == Trigger.OnDoubleClick)
            ChangeToNextSprites();
    }

    void OnSelect(bool isSelected)
    {
        if (!enabled) return;
        if (trigger == Trigger.OnSelect ||
            (trigger == Trigger.OnSelectTrue && isSelected) ||
            (trigger == Trigger.OnSelectFalse && !isSelected))
            ChangeToNextSprites();
    }

    void OnToggle()
    {
        if (!enabled || UIToggle.current == null) return;
        if (trigger == Trigger.OnActivate ||
            (trigger == Trigger.OnActivateTrue && UIToggle.current.value) ||
            (trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
            ChangeToNextSprites();
    }

    void OnDragOver()
    {
        if (enabled && dualState)
        {
            if (UICamera.currentTouch.dragged == gameObject) 
                ChangeToNextSprites();
            else if (buttonTarget.dragHighlight && trigger == Trigger.OnPress)
                ChangeToNextSprites();
        }
    }

    void OnDragOut()
    {
        if (enabled && dualState && UICamera.hoveredObject != gameObject)
            ChangeToNextSprites();
    }

    void OnDrop(GameObject go)
    {
        if (enabled && trigger == Trigger.OnPress && UICamera.currentTouch.dragged != gameObject)
            ChangeToNextSprites();
    }

	#endregion

	#region Metodos

    public void ChangeToNextSprites()
    {
        buttonTarget.normalSprite = sprites[_spritesIndex].normalSprite;
        buttonTarget.hoverSprite = sprites[_spritesIndex].hoverSprite;
        buttonTarget.pressedSprite = sprites[_spritesIndex].pressedSprite;
        buttonTarget.disabledSprite = sprites[_spritesIndex].disabledSprite;

        _spritesIndex++;
        if (_spritesIndex >= sprites.Count)
            _spritesIndex = 0;
    }

	#endregion
}

