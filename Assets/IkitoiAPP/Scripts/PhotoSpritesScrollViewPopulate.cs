using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Vuforia;

[RequireComponent(typeof(UIGrid))]
public class PhotoSpritesScrollViewPopulate : MonoBehaviour
{
	#region Variables (private)
    
	private UIGrid _grid;
    private Transform _markerHelp;

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
		
		var prefab = Resources.Load("Prefabs/GUI/PhotoModelGridItem") as GameObject;
		for (int i = 0; i < ModelsIndex.Models.Count; i++)
		{
			// Model
			var ikiModel = ModelsIndex.Models[i];
			
			// New Grid Item
			var newItem = GameObject.Instantiate(prefab);
						
			// Sprite a mostrar
			var iSprite = newItem.transform.GetChild(0).GetComponent<UISprite>();
			iSprite.atlas = ikiModel.atlas;
			iSprite.spriteName = ikiModel.spriteName;

            // Setea el modelo en el grid item
            var dragDropComp = newItem.GetComponent<UIDragDropModelItem>();
            dragDropComp.model = ikiModel.gameObject;
            if (_markerHelp == null)
                _markerHelp = NGUITools.GetRoot(gameObject).transform.FindChild("Bottom Align - MarkerHelp");
            EventDelegate.Add(dragDropComp.onDropped, () =>
            {
                _markerHelp.gameObject.SetActive(true);
                var renderToggle = _markerHelp.GetComponent<UIRenderToggle>();
                if (!IsTrackableActive())
                    renderToggle.Show();
                else
                {
                    renderToggle.Hide();
                    _markerHelp.FindChild("Text - Panel").GetComponent<ActivationToggle>().Disable();
                }
            });
			
			// Agrega el item y lo ajusta
			_grid.AddChild(newItem.transform);
			newItem.transform.localScale = Vector3.one;
		}
	}

    bool IsTrackableActive()
    {
        // Get the StateManager
        StateManager sm = TrackerManager.Instance.GetStateManager();

        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)
        IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours();

        return activeTrackables.Count() > 0;
    }

	#endregion
}

