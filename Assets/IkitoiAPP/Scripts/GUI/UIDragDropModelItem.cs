using UnityEngine;
using System.Collections.Generic;

public class UIDragDropModelItem : UIDragDropItem
{
    public GameObject model;
    public Vector2 initialScale = Vector2.one;

    public List<EventDelegate> onDropped = new List<EventDelegate>();

	/// <summary>
	/// Drop am sprite object onto the screen.
	/// </summary>

	protected override void OnDragDropRelease (GameObject surface)
	{
        // Remove any previous model
        var container = GameObject.FindGameObjectWithTag("Marker").transform.GetChild(0);
        if (container.transform.childCount > 0)
        {
            for (int i = 0; i < container.transform.childCount; i++)
            {
                Destroy(container.transform.GetChild(i).gameObject);
            }
        }

        // Instantiate the model
        var iModel = Instantiate(model);
        
        // Parent the model to the marker
        var prevScale = iModel.transform.localScale;
        iModel.transform.parent = container.transform;
        iModel.transform.localScale = prevScale;

        // Update layer
        container.GetComponent<ChangeLayer>().UpdateChildren();

        // Animate it to the end
        var cAnimator = iModel.GetComponent<CustomAnimator>();
        cAnimator.playEnding = false;
        cAnimator.SkipAll();

        // Trigger event
        if (EventDelegate.IsValid(onDropped))
            EventDelegate.Execute(onDropped);

		base.OnDragDropRelease(surface);
	}
}
