using Vuforia;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CustomTrackableEventHandler : MonoBehaviour,
                                           ITrackableEventHandler
{      
    //MonoBehaviour Method
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
        
    //Implementacion de ITrackableEventHandler funcion llamada cuando el track cambia de estado
    public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound.Exec(this);
        }
        else
        {
            OnTrackingLost.Exec(this);
        }
    }
    
    //Evento de trackable encontrado
    [HideInInspector]
    public SerializableDelegate OnTrackingFound;

    //Evento de trackable perdido
    [HideInInspector]
    public SerializableDelegate OnTrackingLost;
        
    //Variables
    private TrackableBehaviour mTrackableBehaviour;
}
