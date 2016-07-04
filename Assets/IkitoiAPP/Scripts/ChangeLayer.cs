using UnityEngine;
using System.Collections;

public class ChangeLayer : MonoBehaviour
{
	#region Variables (private)

    public int fromLayer = 0;
    public int toLayer = 0;

    public bool includeChildren = false;

	#endregion

	#region Propiedades (public)

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

    public void ToggleChange()
    {
        if (gameObject.layer == fromLayer)
        {
            SetToLayer();
        }
        else if (gameObject.layer == toLayer)
        {
            SetFromLayer();
        }
    }

    public void SetToLayer()
    {
        gameObject.layer = toLayer;
        if (includeChildren)
            ChangeLayerToChildrens(gameObject.transform, toLayer);
    }

    public void SetFromLayer()
    {
        gameObject.layer = fromLayer;
        if (includeChildren)
            ChangeLayerToChildrens(gameObject.transform, fromLayer);
    }

    public void UpdateChildren()
    {
        if (includeChildren)
        {
            var currLayer = gameObject.layer;
            if (currLayer == fromLayer || currLayer == toLayer)
                ChangeLayerToChildrens(transform, currLayer);
        }
    }

    void ChangeLayerToChildrens(Transform parent, int layer)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            child.gameObject.layer = layer;
            ChangeLayerToChildrens(child, layer);
        }
    }

	#endregion
}

