using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIZoomOnCenter : MonoBehaviour
{
	#region Variables (private)

    private bool _scrollIsMoving;

    private UIScrollView _scrollView;

    private UICenterOnChild _centerOnChild;

	#endregion

	#region Propiedades (public)

    public float deltaScale = 1;

    public float distanceZoomStart = 1;

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        if (_scrollView == null)
            FindScrollView();

        if (_centerOnChild == null)
            FindCenterOnChild();
    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	void Start ()	
	{
        if (_scrollView != null)
        {
            if (_centerOnChild != null) 
                _centerOnChild.onCenter = (GameObject gObject) => this._scrollIsMoving = true;

            _scrollView.onDragStarted = () => this._scrollIsMoving = true;
            _scrollView.onStoppedMoving = () => this._scrollIsMoving = false;
            Zoom();
        }
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	    if (_scrollView && _scrollIsMoving)
        {
            Zoom();
        }
	}

	#endregion

	#region Metodos

    private void FindScrollView()
    {
        if (_scrollView == null)
        {
            _scrollView = NGUITools.FindInParents<UIScrollView>(gameObject);
            if (_scrollView == null)
            {
                Debug.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " on a parent object in order to work", this);
                enabled = false;
                return;
            }
        }
    }

    private void FindCenterOnChild()
    {
        if (_centerOnChild == null)
        {
            _centerOnChild = GetComponent<UICenterOnChild>();
        }
    }

    private void ZoomObject(Transform uiObject, float value)
    {
        uiObject.localScale = Vector3.one + Vector3.one * value * deltaScale;
        foreach (var cSprite in uiObject.GetComponentsInChildren<UISprite>())
        {
            cSprite.MarkAsChanged();
        }
    }

    public void Zoom()
    {
        // Check required components
        if (_scrollView == null)
            FindScrollView();

        // Calculate the panel's center in world coordinates
        Vector3[] corners = _scrollView.panel.worldCorners;
        Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

        // Check childs
        var trans = transform;
        if (trans.childCount == 0) return;
        var grid = GetComponent<UIGrid>();

        // Iterate elements and apply scale
        if (grid != null)
        {
            foreach (var child in grid.GetChildList())
            {
                if (!child.gameObject.activeInHierarchy) continue;
                var dist = Vector3.Distance(child.position, panelCenter);
                if (dist < distanceZoomStart)
                {
                    ZoomObject(child, 1 - (dist / distanceZoomStart));
                }
            }
        }
        else
        {   
            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                if (!child.gameObject.activeInHierarchy) continue;
                var dist = Vector3.Magnitude(child.position - panelCenter);
                if (dist < distanceZoomStart)
                {
                    ZoomObject(child, 1 - (dist / distanceZoomStart));
                }
            }
        }
    }

	#endregion
}

