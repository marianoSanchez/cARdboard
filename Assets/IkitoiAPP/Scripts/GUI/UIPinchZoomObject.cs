//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This script makes it possible to resize the specified widget by dragging on the object this script is attached to.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIPinchZoomObject : MonoBehaviour
{
    struct TouchOrigin
    {
        public int touchID;
        public Ray originRay;
        public Vector3 origin;

        public TouchOrigin(int touchID, Ray originRay)
        {
            this.touchID = touchID;
            this.originRay = originRay;
            this.origin = Vector3.zero;
        }

        public TouchOrigin(int touchID, Ray originRay, Vector3 origin)
        {
            this.touchID = touchID;
            this.originRay = originRay;
            this.origin = origin;
        }
    }

	/// <summary>
	/// Widget that will be dragged.
	/// </summary>

	public UIWidget target;

	/// <summary>
	/// Widget's pivot that will be dragged
	/// </summary>

	private UIWidget.Pivot pivot = UIWidget.Pivot.Center;

	/// <summary>
	/// Minimum width the widget will be allowed to shrink to when resizing.
	/// </summary>

	public int minWidth = 100;

	/// <summary>
	/// Minimum height the widget will be allowed to shrink to when resizing.
	/// </summary>

	public int minHeight = 100;

	/// <summary>
	/// Maximum width the widget will be allowed to expand to when resizing.
	/// </summary>

	public int maxWidth = 100000;

	/// <summary>
	/// Maximum height the widget will be allowed to expand to when resizing.
	/// </summary>

	public int maxHeight = 100000;

    /// <summary>
    /// Zoom scale quantity
    /// </summary>
    
    public Vector2 zoomScale = Vector2.one;

	/// <summary>
	/// If set to 'true', the target object's anchors will be refreshed after each dragging operation.
	/// </summary>

	public bool updateAnchors = false;

    /// <summary>
    /// If set to 'true', the target aspect ratio will remain the same.
    /// </summary>
    
    public bool keepAspectRatio = true;
        
	Plane mPlane;
	Vector3 mLocalPos;
	int mWidth = 0;
	int mHeight = 0;
    float mAspectRatio;

    int currentTouchs = 0;
    List<Vector3> mDeltaRays = new List<Vector3>();
    List<TouchOrigin> mTouchs = new List<TouchOrigin>();

    bool IsBeingPinched()
    {
        return currentTouchs == 2;
    }
    
    void OnPress (bool pressed)
    {
        if (target == null) return;

        if (pressed)
        {
            var ray = UICamera.currentRay;
            mTouchs.Add(new TouchOrigin(UICamera.currentTouchID, ray));
            currentTouchs++;            
        }
        else
        {
            mTouchs.RemoveAll(x => x.touchID == UICamera.currentTouchID);
            currentTouchs--;
        }

        if (IsBeingPinched())
        {
            Vector3[] corners = target.worldCorners;
            mPlane = new Plane(corners[0], corners[1], corners[3]);
            mLocalPos = target.cachedTransform.localPosition;
            mWidth = target.width;
            mHeight = target.height;
            mAspectRatio = target.aspectRatio;

            for (int i = 0; i < 2; i++)
            {
                float dist;
                var ray = mTouchs[i].originRay;
                if (mPlane.Raycast(ray, out dist))
                {
                    var touchOrigin = mTouchs[i];
                    touchOrigin.origin = ray.GetPoint(dist);
                    mTouchs[i] = touchOrigin;
                }
                else
                {
                    mTouchs.RemoveAt(i);
                    currentTouchs--;
                    break;
                }
            }

            mDeltaRays = new List<Vector3>() { Vector3.zero, Vector3.zero };
        }
    }

	/// <summary>
	/// Adjust the widget's dimensions.
	/// </summary>

	void OnDrag (Vector2 delta)
	{
        if (IsBeingPinched())
		{
			float dist;
			Ray ray = UICamera.currentRay;

			if (mPlane.Raycast(ray, out dist))
			{
                var cTouchIndex = mTouchs.IndexOf(mTouchs.Where(x => x.touchID == UICamera.currentTouchID).First());
                var cTouchOrigin = mTouchs[cTouchIndex].origin;

                // Move the widget
                var deltaPos = GetLocalDelta(cTouchOrigin, ray.GetPoint(dist));

                // Invert the movement for the left component
                var touchBOrigin = mTouchs.Where(x => x.touchID != UICamera.currentTouchID).First().origin;
                if (cTouchOrigin.x < touchBOrigin.x)
                    deltaPos.x *= -1;
                if (cTouchOrigin.y < touchBOrigin.y)
                    deltaPos.y *= -1;
                
                // Store the delta value
                mDeltaRays[cTouchIndex] = deltaPos;
			}
		}
	}

    void LateUpdate()
    {
        if (IsBeingPinched())
        {
            var t = target.cachedTransform;
            var localDelta = Vector3.zero;
            
            // Calculate delta
            localDelta.x = (mDeltaRays[0].x + mDeltaRays[1].x) * zoomScale.x;
            localDelta.y = (mDeltaRays[0].y + mDeltaRays[1].y) * zoomScale.y;

            // Calculate the final delta (apply rotation) and calculate final local size
            localDelta = Quaternion.Inverse(t.localRotation) * localDelta;
            var localSize = new Vector2(localDelta.x + mWidth, localDelta.y + mHeight);

            // Clamp values
            localSize.x = Mathf.Clamp(localSize.x, minWidth, maxWidth);
            localSize.y = Mathf.Clamp(localSize.y, minHeight, maxHeight);

            // Keep aspect ratio?
            if (keepAspectRatio)
            {
                bool resizedX;

                // Apply proportions
                if (localSize.x > localSize.y * mAspectRatio)
                {
                    localSize.y = localSize.x / mAspectRatio;
                    resizedX = false;
                }
                else
                {
                    localSize.x = localSize.y * mAspectRatio;
                    resizedX = true;
                }

                // ReCheck Aspect Ratio
                if (resizedX)
                {
                    if (localSize.x > maxWidth)
                    {
                        localSize.x = maxWidth;
                        localSize.y = localSize.x / mAspectRatio;
                    }
                }
                else
                {                    
                    if (localSize.y > maxHeight)
                    {
                        localSize.y = maxHeight;
                        localSize.x = localSize.y * mAspectRatio;
                    }
                }
            }

            // Resize sprite and collider
            NGUIMath.ResizeWidget(target, pivot, localSize.x, localSize.y, minWidth, minHeight);
            
            // Update all anchors
            if (updateAnchors) target.BroadcastMessage("UpdateAnchors");
        }
    }

    Vector3 GetLocalDelta(Vector3 worldA, Vector3 worldB)
    {
        var t = target.cachedTransform;
        t.localPosition = mLocalPos;

        // Move the widget
        var worldDelta = worldB - worldA;
        t.position = t.position + worldDelta;
        var deltaPos = t.localPosition - mLocalPos;

        // Restore the position
        t.localPosition = mLocalPos;

        return deltaPos;
    }
}
