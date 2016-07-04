using UnityEngine;
using System.Collections;

public class UIAskForPhotoPopUp : UIPopUp
{
	#region Variables (private)

	#endregion

	#region Propiedades (public)
    
	#endregion

	#region Funciones de evento de Unity
    
    void Start()
    {
        EventDelegate.Add(this.onAccept, () => IkitoiAPP.Instance.GoToPhoto());
    }

	#endregion

	#region Metodos
        
	#endregion
}

