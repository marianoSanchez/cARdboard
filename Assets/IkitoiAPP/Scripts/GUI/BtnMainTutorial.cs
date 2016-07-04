using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIButton))]
public class BtnMainTutorial : MonoBehaviour
{
	#region Variables (private)
    
	private UIButton _btnComp;

	#endregion

	#region Propiedades (public)

    [HideInInspector]
	public bool runOnce = true;

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    void Awake()
    {
		if (_btnComp == null)
			_btnComp = GetComponent<UIButton>();
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

	void OnClick()
	{
		_btnComp.onClick.Clear();
		EventDelegate.Add(_btnComp.onClick, IkitoiMenuGUI.Instance.GoToModels);
		if (runOnce)
			this.enabled = false;
	}

	#endregion

	#region Metodos

	#endregion
}

