using UnityEngine;
using System.Collections;

public class UIPartsWindow : MonoBehaviour
{
	#region Variables (private)
    
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

	void OnEnable()
	{
		var partsNumbers = IkitoiAPP.SelectedModel.GetComponent<ModelParts>().Parts;
		for (int i = 0; i < partsNumbers.Count; i++)
		{
			var child = transform.GetChild(i);
			if (child == null) break;

			var label = child.GetComponentInChildren<UILabel>();
			label.text = partsNumbers[i].ToString();
		}
	}

	#endregion

	#region Metodos

	#endregion
}

