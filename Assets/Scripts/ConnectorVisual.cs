using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the visuals for the connector system. 
/// (Incorporates rudimentary object pooling)
/// </summary>
public class ConnectorVisual : MonoBehaviour {

	public List<LineRenderer> lineRenderers = new List<LineRenderer>();
	public GameObject linePrefab;
	private LineRenderer activeLine;

	/// <summary>
	/// Start a new line from the provided jewel
	/// </summary>
	public void DrawNewLine(Jewel startingJewel)
	{
		activeLine = GetLine();
		Vector3 adjustedPosition = startingJewel.transform.position;
		adjustedPosition.z = -1f;
		activeLine.SetPosition(0, adjustedPosition);
	}

	/// <summary>
	/// Set the end of the current line position to the provided jewel
	/// </summary>
	public void SetLineEndPosition(Jewel endPosition)
	{
		Vector3 adjustedPosition = endPosition.transform.position;
		adjustedPosition.z = -1f;
		activeLine.SetPosition(activeLine.positionCount - 1, adjustedPosition);
	}

	/// <summary>
	/// Set the end of the current line position to the current touch or mouse position
	/// </summary>
	public void SetLineEndMouseTouchPosition()
	{
		if (activeLine != null)
		{
			Vector3 position =  Vector3.zero;
			if (Input.touchSupported)
			{
				position = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Vector3.zero;
			}
			else
			{
				position = Input.mousePosition;
			}
			position.z = -1f;
			activeLine.SetPosition(activeLine.positionCount - 1, 
				Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	/// <summary>
	/// Clears (deactivates) all lines.
	/// </summary>
	public void Clear()
	{
		for (int i = 0; i < lineRenderers.Count; i++)
		{
			lineRenderers[i].gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Gets an inactive line if there is one. Creates a new one if not.
	/// </summary>
	private LineRenderer GetLine()
	{
		for (int i = 0; i < lineRenderers.Count; i++)
		{
			if (!lineRenderers[i].gameObject.activeSelf)
			{
				lineRenderers[i].gameObject.SetActive(true);
				return lineRenderers[i];
			}
		}
		LineRenderer newLine = Instantiate(linePrefab, transform).GetComponent<LineRenderer>();
		lineRenderers.Add(newLine);
		return newLine;
	}
}
