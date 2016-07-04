using UnityEngine;
using System.Collections;

public class UIConfirmationDialog : UIPopUp
{
	#region Variables (private)

    private UILabel _label;

	#endregion

	#region Propiedades (public)

    public string Description
    {
        get
        {
            if (_label == null)
                _label = transform.GetComponentInChildren<UILabel>();

            return _label.text;
        }
        set
        {
            if (_label == null)
                _label = transform.GetComponentInChildren<UILabel>();

            _label.text = value;
        }
    }

	#endregion

	#region Funciones de evento de Unity

    /// <summary>
    /// Llamado siempre al inicializar el componente.
    /// </summary>
    protected override void Awake()
    {

    }

	/// <summary>
    /// Llamado al inicializar el componente, si MonoBehaviour esta habilitado.
	/// </summary>
	protected void Start()
    {
        base.Awake();
	}

    void OnDisable()
    {
        Destroy(this.gameObject);
    }

	#endregion

	#region Metodos

    public void Initialize()
    {
        if (_label == null)
            _label = transform.GetComponentInChildren<UILabel>();

        base.Awake();
    }

    public static UIConfirmationDialog CreateDialog(Transform parent)
    {
        var dialogPrefab = Resources.Load("Prefabs/GUI/ConfirmationDialog") as GameObject;
        var newDialog = GameObject.Instantiate(dialogPrefab);
        
        // Lo centra respecto a la pantalla
        var UIRoot = NGUITools.GetRoot(parent.gameObject).GetComponent<UIRoot>();
        newDialog.transform.parent = UIRoot.transform;
        newDialog.transform.localPosition = Vector3.zero;
        newDialog.transform.localScale = Vector3.one;
        newDialog.transform.localRotation = Quaternion.identity;
        
        // Lo ubica en la jerarquia
        newDialog.transform.parent = parent;

        return newDialog.GetComponent<UIConfirmationDialog>();
    }

	#endregion
}

