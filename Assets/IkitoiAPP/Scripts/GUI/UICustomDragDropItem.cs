using UnityEngine;

public class UICustomDragDropItem : UIDragDropItem
{
    [SerializeField]
    private Transform _spritesContainer;

    public Vector2 initialScale = Vector2.one;

	/// <summary>
	/// Drop am sprite object onto the screen.
	/// </summary>

	protected override void OnDragDropRelease (GameObject surface)
	{
        // Create a clone of the sprite
        var spriteObject = Instantiate(transform.GetChild(0));

        // Apply depht of sprite
        var sprite = spriteObject.GetComponent<UISprite>();
        sprite.depth = _spritesContainer.childCount;

        // Parent the sprite to the container
        spriteObject.parent = _spritesContainer;
        var pos = mTrans.localPosition;
        pos.z = 0f;
        spriteObject.localPosition = mTrans.localPosition;
        spriteObject.localScale = Vector3.one;

        // Apply scale to the sprite
        NGUIMath.ResizeWidget(sprite, UIWidget.Pivot.Center, sprite.width * initialScale.x, sprite.height * initialScale.y, 0, 0);

        // Enable the collider comp
        spriteObject.GetComponent<BoxCollider>().enabled = true;

		base.OnDragDropRelease(surface);
	}
}
