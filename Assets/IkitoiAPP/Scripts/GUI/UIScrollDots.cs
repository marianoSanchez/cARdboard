using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScrollDots : MonoBehaviour
{
	#region Variables (private)

    [SerializeField]
    private GameObject _dot;
    [SerializeField]
    private GameObject _thumb;

    private int _currentItem = -1;
    private bool _isDirty = false;
    private bool _scrollIsMoving = false;

    private Transform _iThumb;
    private Transform _containerGrid;

	#endregion

	#region Propiedades (public)

    public UIScrollView scrollView;

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
        // Busca el contenedor de los Dots
        if (_containerGrid == null)
            FindCointainerGrid();
        
        // Checkea que se halla especificado un scrollview a monitorear
        if (scrollView == null)
            NoScrollView();

        // Regenera los dots
        RegenerateDots();
    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	void Start ()	
	{
        // Eventos que indicaran la necesidad de chequear por cambios en el item seleccionado
        scrollView.onDragStarted = () => this._scrollIsMoving = true;
        scrollView.onStoppedMoving = () => this._scrollIsMoving = false;

        var scrollGrid = scrollView.transform.GetChild(0).GetComponent<UIGrid>();
        scrollGrid.onReposition += () => this.RegenerateDots();
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update ()
    {
        if (_scrollIsMoving || _isDirty)
        {
            var closest = GetClosestItem();
            if (_currentItem != closest)
            {
                ChangeCurrentItem(closest);
            }
        }
	}

	#endregion

	#region Metodos

    public void MarkAsDirty()
    {
        this._isDirty = true;
    }

    private void FindCointainerGrid()
    {
        var grid = gameObject.GetComponent<UIGrid>();
        if (grid == null)
            grid = gameObject.GetComponentInChildren<UIGrid>();

        if (grid == null)
            {
            Debug.LogWarning(GetType() + " requires " + typeof(UIGrid) + " on this object or a child in order to work", this);
            enabled = false;
            return;
        }

        _containerGrid = grid.transform;
    }
    
    private void NoScrollView()
    {
        Debug.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " on a parent object in order to work", this);
        enabled = false;
        return;
    }
    
    private int GetClosestItem()
    {
        Transform trans = transform;

        // Calculate the panel's center in world coordinates
        Vector3[] corners = scrollView.panel.worldCorners;
        Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

        // Offset this value by the momentum
        Vector3 momentum = scrollView.currentMomentum * scrollView.momentumAmount;
        Vector3 moveDelta = NGUIMath.SpringDampen(ref momentum, 9f, 2f);
        Vector3 pickingPoint = panelCenter - moveDelta * 0.01f; // Magic number based on what "feels right"


        var scrollChild = scrollView.transform.GetChild(0);
        var scrollGrid = scrollChild.GetComponent<UIGrid>();
        List<Transform> list = null;

        int closest = 0;
        float min = float.MaxValue;

        // Determine the closest child
        if (scrollGrid != null)
        {
            list = scrollGrid.GetChildList();

            for (int i = 0, imax = list.Count, ii = 0; i < imax; ++i)
            {
                Transform t = list[i];
                if (!t.gameObject.activeInHierarchy) continue;
                float sqrDist = Vector3.SqrMagnitude(t.position - pickingPoint);

                if (sqrDist < min)
                {
                    min = sqrDist;
                    closest = i;
                }
                ++ii;
            }
        }
        else
        {
            for (int i = 0, imax = trans.childCount, ii = 0; i < imax; ++i)
            {
                Transform t = trans.GetChild(i);
                if (!t.gameObject.activeInHierarchy) continue;
                float sqrDist = Vector3.SqrMagnitude(t.position - pickingPoint);

                if (sqrDist < min)
                {
                    min = sqrDist;
                    closest = i;
                }
                ++ii;
            }
        }

        return closest;
    }

    private void RegenerateDots()
    {
        var scrollGrid = scrollView.transform.GetChild(0);

        if (scrollGrid.childCount == 0)
        {
            _containerGrid.DestroyChildren();
        }
        else if (scrollGrid.childCount != _containerGrid.childCount)
        {
            _containerGrid.DestroyChildren();

            for (int i = 0; i < scrollGrid.childCount; i++)
                AddDot();
            AddThumb();

            _containerGrid.GetComponent<UIGrid>().Reposition();

            MarkAsDirty();
        }
    }

    private void AddDot()
    {
        var newDot = Instantiate<GameObject>(_dot).transform;
        newDot.parent = _containerGrid;
        newDot.localScale = Vector3.one;
    }

    private void AddThumb()
    {
        _iThumb = Instantiate<GameObject>(_thumb).transform;
        _iThumb.parent = _containerGrid.GetChild(0);
        _iThumb.localScale = Vector3.one;
        _iThumb.localPosition = Vector3.zero;
    }

    private void ChangeCurrentItem(int newCurrent)
    {
        _currentItem = newCurrent;

        _iThumb.transform.parent = _containerGrid.GetChild(_currentItem);
        _iThumb.localPosition = Vector3.zero;
    }

	#endregion
}

