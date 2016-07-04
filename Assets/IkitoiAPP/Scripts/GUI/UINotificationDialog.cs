using UnityEngine;
using System.Collections;

public class UINotificationDialog : UIPopUp
{
	#region Variables (private)


	#endregion

	#region Propiedades (public)

    [Space(12)]
    public UILabel label;

    public string Description
    {
        get
        {
            if (label == null)
                label = transform.GetComponentInChildren<UILabel>();

            return label.text;
        }
        set
        {
            if (label == null)
                label = transform.GetComponentInChildren<UILabel>();

            label.text = value;
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
        if (label == null)
            label = transform.GetComponentInChildren<UILabel>();

        base.Awake();
    }

    public void Finish()
    {
        var dots = transform.GetChild(0).GetChild(0);
        dots.gameObject.SetActive(false);

        var messageBody = transform.GetChild(0).GetChild(1);
        messageBody.gameObject.SetActive(true);
    }

    public static UINotificationDialog CreateDialog(Transform parent, bool finished = false)
    {
        var dialogPrefab = Resources.Load("Prefabs/GUI/NotificationDialog") as GameObject;
        var newDialog = GameObject.Instantiate(dialogPrefab);
        
        // Lo centra respecto a la pantalla
        var UIRoot = NGUITools.GetRoot(parent.gameObject).GetComponent<UIRoot>();
        newDialog.transform.parent = UIRoot.transform;
        newDialog.transform.localPosition = Vector3.zero;
        newDialog.transform.localScale = Vector3.one;
        newDialog.transform.localRotation = Quaternion.identity;
        
        // Lo ubica en la jerarquia
        newDialog.transform.parent = parent;

        // Extrae el comoponente
        var dialogComp = newDialog.GetComponent<UINotificationDialog>();

        // Lo finaliza si se indica
        if (finished)
            dialogComp.Finish();

        return dialogComp;
    }

	#endregion
}

