using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour 
{

	[Range(0, 40), SerializeField]
	private byte rows, columns = 8;
	[SerializeField]
	private GameObject jewelPrefab;
	[SerializeField]
	private float jewelSpacing = 1f;

	private List<Jewel> jewels = new List<Jewel>();

	public static GridManager Instance
	{
		get { return instance; }
		set { instance = value; }
	}
	private static GridManager instance;

	private void Awake()
	{
		instance = GetComponent<GridManager>();
		CreateJewels();
	}

	private void Start()
	{
		//Animate the jewels on start
		ExecuteJewelOperation( (jewel) => 
		{
				jewel.transform.position = GetPositionForCoordinates(jewel.coordinates);
				jewel.MoveToFromOutsideCamera(((jewel.coordinates.row)/5f)+(jewel.coordinates.column/5f)/10f);

		});
	}
		
	/// <summary>
	/// Instantiates and spawns new jewels.
	/// </summary>
	public void CreateJewels()
	{
		//For each column and row, spawn a new jewel. Set their sprite, type and coordinate.
		for (byte column = 0; column < columns; column++)
		{
			for (byte row = 0; row < rows; row++)
			{
				var jewel = Instantiate(jewelPrefab, transform).GetComponent<Jewel>();
				jewel.JewelType = (JewelType)Random.Range(0,5);
				jewel.SetSprite();
				jewel.coordinates = new Coordinate(row, column);				
				jewels.Add(jewel);
			}
		}
	}

	/// <summary>
	/// Clears any jewels of type Cleared and spawns new ones.
	/// </summary>
	public void ClearAndSpawnJewels(List<Jewel> activeConnections)
	{
		//Cache which columns that has been changed and set the jewels to cleared
		var clearColumn = new bool[columns];
		for (byte i = 0; i < activeConnections.Count; i++){
			
			clearColumn[activeConnections[i].coordinates.column] = true;
			activeConnections[i].JewelType = JewelType.Cleared;
		}

		//Set the score
		UIController.Instance.AddScore(activeConnections.Count);

		//Loop through columns and operate the changed ones
		for (byte column = 0; column < columns; column++)
		{
			if (clearColumn[column] == false){
				continue;
			}

			//Handle the non-cleared jewels first. Loop through, assign new coordinates and animate.
			ExecuteJewelColumnOperation(column, (jewel) => 
				{
					if (jewel.JewelType == JewelType.Cleared)
					{
						jewel.gameObject.SetActive(false);
					}
					else
					{
						jewel.coordinates.row -= AmountRemovedBelow(jewel.coordinates);
						jewel.MoveTo(GetPositionForCoordinates(jewel.coordinates));
					}

				});
			//Second, with the new coordinates set, handle the cleared jewels. Assign new coordinates, set sprite, spawn and animate.
			byte moveCount = 0;
			ExecuteJewelColumnOperation(column, (jewel) => 
				{
					if (jewel.JewelType == JewelType.Cleared)
					{
						jewel.coordinates.row = (byte)((rows - 1) - moveCount);
						jewel.gameObject.SetActive(false);


						jewel.JewelType = (JewelType)Random.Range(0,5);
						jewel.transform.transform.position = GetPositionForCoordinates(jewel.coordinates);
						jewel.SetSprite();
						jewel.MoveToFromOutsideCamera(0.1f*(jewel.coordinates.row+1));

						moveCount++;
					}

				});
		}
	}
		
	/// <summary>
	/// Returns the amount of jewels that has been removed below the provided coordinate
	/// </summary>
	private byte AmountRemovedBelow(Coordinate coordinate)
	{
		byte removeCount = 0;
		ExecuteJewelColumnOperation(coordinate.column, (jewel) => 
			{
				if (jewel.coordinates.row < coordinate.row && jewel.JewelType == JewelType.Cleared)
				{
					removeCount++;
				}

			});
		return removeCount;
	}

	/// <summary>
	/// Gets the world position based on the provided coordinate
	/// </summary>
	private Vector2 GetPositionForCoordinates(Coordinate coordinate)
	{
		float x = -jewelSpacing * ((columns - 1) / 2f);
		x += jewelSpacing * coordinate.column;
		float y = -jewelSpacing * ((rows - 1) / 2f);
		y += jewelSpacing * coordinate.row;

		return new Vector2(x, y);
	}
		
	delegate void OnJewelOperation(Jewel jewel);
	/// <summary>
	/// Execute operation on all jewels within a column
	/// </summary>
	private void ExecuteJewelColumnOperation(byte column, OnJewelOperation callback)
	{
		for (int row = column * rows; row < (column * rows) + rows; row++)
		{
			callback(jewels[row]);
		}
	}

	/// <summary>
	/// Execute operation on all jewels
	/// </summary>
	private void ExecuteJewelOperation(OnJewelOperation callback)
	{
		foreach (var d in jewels)
		{
			callback(d);
		}
	}
}