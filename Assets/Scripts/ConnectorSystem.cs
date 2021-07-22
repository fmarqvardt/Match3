using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens to jewel IO and handling of active connections.
/// </summary>
public class ConnectorSystem : MonoBehaviour {

	private ConnectorVisual connectorVisual;
	private JewelType activeSelectionType;
	private List<Jewel> activeSelections = new List<Jewel>();

	void Awake()
	{
		connectorVisual = GetComponent<ConnectorVisual>();
	}


	void Start () 
	{
		JewelIO.SelectionStarted += OnConnectionStarted;
		JewelIO.NewSelection += OnNewSelection;
		JewelIO.SelectionEnded += OnSelectionEnded;
	}
		
	void OnConnectionStarted(Jewel jewel)
	{
		
		activeSelections.Add(jewel);
		activeSelectionType = jewel.JewelType;

		//Animate & draw line
		jewel.PunchScale();
		connectorVisual.DrawNewLine(jewel);
	}
		
	void OnNewSelection(Jewel jewel)
	{
		//If its the same type as the current type and it doesnt contain it already.
		if (jewel.JewelType == activeSelectionType && !activeSelections.Contains(jewel)){
			//If its not a valid neighbour, return.
			if (!activeSelections[activeSelections.Count - 1].IsValidNeighbours(jewel)){
				return;
			}
			//Add to active selection
			activeSelections.Add(jewel);

			//Animate & draw line
			jewel.PunchScale();
			int previousJewelIdx = Mathf.Clamp(activeSelections.Count - 1, 0, int.MaxValue);
			connectorVisual.SetLineEndPosition(activeSelections[previousJewelIdx]);
			connectorVisual.DrawNewLine(activeSelections[previousJewelIdx]);
		}
	}
		
	void OnSelectionEnded(Jewel jewel)
	{
		//If we have enough valid connections then send them off for clearing & new spawn
		if (activeSelections.Count > 2){
			GridManager.Instance.ClearAndSpawnJewels(activeSelections);
		}
		//Reset
		ClearSelectionsAndVisuals();
	}

	//Update visuals of the line connector and if selection ended outside Jewel.
	void Update()
	{
		//If we have enough valid connections then send them off for clearing & new spawn
		if (Input.GetMouseButtonUp(0))
		{
			if (activeSelections.Count > 2){
				GridManager.Instance.ClearAndSpawnJewels(activeSelections);
			}
			ClearSelectionsAndVisuals();
		}
		connectorVisual.SetLineEndMouseTouchPosition();
	}

	/// <summary>
	/// Get the current pointer position in world (z: 0)
	/// </summary>
	/// <returns>Mouse position or first touch position</returns>
	private Vector2 GetPointerWorldPosition()
	{
		var screen = Vector2.zero;
		if (Input.touchSupported)
		{
			screen = Input.touchCount > 0 ? Input.GetTouch(0).position : Vector2.zero;
		}
		else
		{
			screen = Input.mousePosition;
		}
		return Camera.main.ScreenToWorldPoint(screen);
	}

	void ClearSelectionsAndVisuals()
	{
		activeSelections.Clear();
		connectorVisual.Clear();
	}
}
