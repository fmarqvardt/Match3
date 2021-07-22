using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jewel : JewelIO
{
	public Coordinate coordinates;
	[SerializeField]
	private DataHolder data;

	private Tween tween;

	private JewelType jewelType;
	public JewelType JewelType
	{
		set { jewelType = value; }
		get { return jewelType; }
	}

	/// <summary>
	/// Sets the sprite based on the jewels JewelType
	/// </summary>
	public void SetSprite()
	{
		//Pull sprite from the "database".
		GetComponent<SpriteRenderer>().sprite = data.GetSprite((byte)jewelType);
	}

	/// <summary>
	/// Move the sprite from it's current position to the provided position
	/// </summary>
	public void MoveTo(Vector3 position)
	{
		//If there is an actual change in position
		if (position != transform.position)
		{
			//Kill any running tweens
			if (tween != null){
				tween.Kill();
			}

			gameObject.SetActive(true);
			tween = transform.DOLocalMove(position, 0.5f).SetEase(Ease.OutBounce);
		}
	}

	/// <summary>
	/// Will return true if the second jewel is within one hex of the first.
	/// </summary>
	public bool IsValidNeighbours(Jewel secondJewel)
	{
		bool left = secondJewel.coordinates.column > coordinates.column - 2;
		bool right = secondJewel.coordinates.column < coordinates.column + 2;
		bool top = secondJewel.coordinates.row > coordinates.row - 2;
		bool bottom = secondJewel.coordinates.row < coordinates.row + 2;
		return left&&right&&top&&bottom;
	}

	/// <summary>
	/// Moves the jewel to it's current position from outside of the camera bounds.
	/// </summary>
	public void MoveToFromOutsideCamera(float delay = 0f)
	{
		gameObject.SetActive(true);
		float currentPosition = transform.position.y;
		transform.position = new Vector2(transform.position.x, Camera.main.orthographicSize + 2f);
		tween = transform.DOLocalMoveY(currentPosition, 0.5f).SetEase(Ease.OutBounce).SetDelay(delay);
	}

	/// <summary>
	/// Punches the jewels scale.
	/// </summary>
	public void PunchScale()
	{
		transform.DOPunchScale(transform.localScale * 0.4f, 0.5f, 7, 0.7f);
	}
}
