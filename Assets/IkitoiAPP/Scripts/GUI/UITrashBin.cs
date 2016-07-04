using UnityEngine;
using System.Collections;

public class UITrashBin : MonoBehaviour
{
	#region Variables (private)

    Vector3 _lastTouchPos;
    bool _overTrashbin = false;
    bool _showing = false;
    bool _isDragging = false;
    Transform _draggingItem = null;

	#endregion

	#region Propiedades (public)

    public UIWidget _trashBinWidget;
    public Transform trashabeItemsContainer;

    public bool Showing
    {
        get { return _showing; }
        set { _showing = value; }
    }

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
        if (trashabeItemsContainer != null)
        {
            CheckDraggingItems();

            // Comenzo a arrastrar un elemento?
            if (_isDragging && !_showing)
            {
                ShowTrashbin();
            }
            // Esta arrastrando el elemento?
            else if (_isDragging && _showing)
            {
                if (IsOverTrashbin() && !_overTrashbin)
                {
                    OnEnter();
                    _overTrashbin = true;
                }
                else if (!IsOverTrashbin() && _overTrashbin)
                {
                    OnExit();
                    _overTrashbin = false;
                }
            }
            // Solto el elemento?
            else if (!_isDragging && _showing)
            {
                // Drop over trashbin
                if (IsOverTrashbin())
                {
                    if (_draggingItem == null) return;

                    _draggingItem.GetComponent<BoxCollider>().enabled = false;
                    _draggingItem.transform.parent = _trashBinWidget.transform;

                    var pTweener = _draggingItem.gameObject.AddComponent<TweenPosition>();
                    pTweener.from = _draggingItem.localPosition;
                    pTweener.to = Vector3.zero;
                    pTweener.duration = .25f;

                    var sTweener = _draggingItem.gameObject.AddComponent<TweenScale>();
                    sTweener.from = _draggingItem.localScale;
                    sTweener.to = Vector3.zero;
                    sTweener.duration = .25f;

                    EventDelegate.Add(sTweener.onFinished, () =>
                    {
                        Destroy(_draggingItem.gameObject);
                        _draggingItem = null;
                        HideTrashbin();
                    });

                    pTweener.PlayForward();
                    sTweener.PlayForward();
                }
                // Drop somewhere else
                else
                {
                    HideTrashbin();
                }
            }
        }
	}

    void OnEnter()
    {
        var tColor = NGUITools.AddMissingComponent<TweenColor>(_draggingItem.gameObject);

        tColor.from = Color.white;
        tColor.to = Color.red;
        tColor.duration = .25f;
        tColor.ResetToBeginning();
        tColor.PlayForward();
    }

    void OnExit()
    {
        var tColor = NGUITools.AddMissingComponent<TweenColor>(_draggingItem.gameObject);

        tColor.from = Color.red;
        tColor.to = Color.white;
        tColor.duration = .25f;
        tColor.ResetToBeginning();
        tColor.PlayForward();
    }

	#endregion

	#region Metodos

    bool IsOverTrashbin()
    {
        // Local bounds of the trashbin
        //var trashBounds = _trashBinWidget.CalculateBounds();
        //trashBounds.extents = trashBounds.extents + Vector3.forward;
        
        var trashCollider = _trashBinWidget.GetComponent<BoxCollider>();
        var trashBounds = new Bounds(Vector3.zero, new Vector3(trashCollider.size.x, trashCollider.size.y, 1f));

        //var collider = _trashBinWidget.GetComponent<BoxCollider>();
        //var trashBounds2 = _trashBinWidget.GetComponent<BoxCollider>().bounds;

        // Position of the touch relative to the trashbin
        var localTouchPos = _trashBinWidget.transform.InverseTransformPoint(_lastTouchPos);

        // Check collision of touch against trashbin
        return trashBounds.Contains(localTouchPos);
    }

    void ShowTrashbin()
    {
        _showing = true;

        _trashBinWidget.enabled = true;

        var tColor = NGUITools.AddMissingComponent<TweenAlpha>(_trashBinWidget.gameObject);

        tColor.from = _trashBinWidget.alpha;
        tColor.to = 1f;
        tColor.duration = .2f;
        tColor.ResetToBeginning();
        tColor.PlayForward();
    }

    void HideTrashbin()
    {
        var tColor = NGUITools.AddMissingComponent<TweenAlpha>(_trashBinWidget.gameObject);

        tColor.from = _trashBinWidget.alpha;
        tColor.to = 0f;
        tColor.duration = .2f;
        tColor.ResetToBeginning();
        EventDelegate.Add(tColor.onFinished, () =>
        {
            _trashBinWidget.enabled = false;
            _showing = false;
        },
        true);

        tColor.PlayForward();
    }

    void CheckDraggingItems()
    {
        _isDragging = false;
        for (int i = 0; i < trashabeItemsContainer.childCount; i++)
        {
            var item = trashabeItemsContainer.GetChild(i);
            var dragComp = item.GetComponent<UICustomDragObject>();

            if (dragComp == null) continue;

            if (dragComp.IsPressed(1))
            {
                _lastTouchPos = UICamera.lastHit.point;
                _draggingItem = item;
                _isDragging = true;
                break;
            }
        }
    }

	#endregion
}

