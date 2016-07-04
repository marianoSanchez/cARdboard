using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIGrid))]
public class ModelsScrollViewPopulate : MonoBehaviour
{
	#region Variables (private)
    
	private UIGrid _grid;
	private UIZoomOnCenter _zoomOnCenter;

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
		RePopulate();
	}

	/// <summary>
    /// Update es llamado una vez por frame, si MonoBehaviour esta habilitado.
	/// </summary>
	void Update () 
	{
	
	}

	#endregion

	#region Metodos

	public void RePopulate()
	{
		if (_grid == null)
			_grid = GetComponent<UIGrid>();
		
		var prefab = Resources.Load("Prefabs/GUI/ModelsGridItem") as GameObject;
		for (int i = 0; i < ModelsIndex.Models.Count; i++)
		{
			// Model
			var ikiModel = ModelsIndex.Models[i];
			
			// New Grid Item
			var newItem = GameObject.Instantiate(prefab);
			
			// Comportamiento en click
            var iButton = newItem.GetComponent<UIButton>();
            EventDelegate.Add(iButton.onClick, () =>
			                  {
				IkitoiAPP.SelectedModel = ikiModel.gameObject;
                IkitoiMenuGUI.Instance.Parts.Show();
            });
			
			// Sprite a mostrar
			var iSprite = newItem.GetComponentInChildren<UISprite>();
			iSprite.atlas = ikiModel.atlas;
			iSprite.spriteName = ikiModel.spriteName;
			
			// Agrega el item y lo ajusta
			_grid.AddChild(newItem.transform);
			newItem.transform.localScale = Vector3.one;
		}
				
		if (_zoomOnCenter == null)
			_zoomOnCenter = GetComponent<UIZoomOnCenter>();
		
		if (_zoomOnCenter != null && _zoomOnCenter.isActiveAndEnabled)
			_zoomOnCenter.Zoom();
	}

	#endregion
}

