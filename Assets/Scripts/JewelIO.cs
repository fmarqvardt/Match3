using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input/Output for jewels.
/// </summary>
public class JewelIO : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler {

	Jewel jewel;
	public static bool hasSelection = false;

	void Awake()
	{
		jewel = GetComponent<Jewel>();
	}

	#region Events
	public delegate void OnSelectionEnded(Jewel jewel);
	public static event OnSelectionEnded SelectionEnded;
	public void OnPointerUp (PointerEventData eventData)
	{
		hasSelection = false;
		SelectionEnded(jewel);
	}


	public delegate void OnNewSelection(Jewel jewel);
	public static event OnNewSelection NewSelection;
	public void OnPointerEnter (PointerEventData eventData)
	{
		if (hasSelection){
			NewSelection(jewel);
		}
	}

	public delegate void OnSelectionStarted(Jewel jewel);
	public static event OnSelectionStarted SelectionStarted;
	public void OnPointerDown (PointerEventData eventData)
	{
		hasSelection = true;
		SelectionStarted(jewel);
	}

	#endregion
}
