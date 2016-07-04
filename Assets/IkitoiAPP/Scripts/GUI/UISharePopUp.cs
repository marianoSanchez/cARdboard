using UnityEngine;
using System.Collections;

public class UISharePopUp : UIPopUp
{
	#region Variables (private)

    [Space(10)]
    [SerializeField]
    public UIInput _descriptionInput;

	#endregion

	#region Propiedades (public)

    public string Description
    {
        get
        {
            return "Captura de IkitoiAPP!";//_descriptionInput.label.text;
        }
    }

	#endregion

	#region Funciones de evento de Unity
    
	#endregion

	#region Metodos

	#endregion
}

