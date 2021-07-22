using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataBase for easy hotswapping of sprites, sound, etc.
/// </summary>
public class DataHolder : ScriptableObject {
	[SerializeField]
	private Sprite[] jewelSprites = new Sprite[0];

	/// <summary>
	/// Get sprite by index.
	/// </summary>
	public Sprite GetSprite(byte idx)
	{
		return jewelSprites[idx];
	}
}
