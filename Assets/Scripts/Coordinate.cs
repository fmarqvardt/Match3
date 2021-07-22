/// <summary>
/// Simple struct which holds the row and column coordinates.
/// </summary>
public struct Coordinate
{
	public byte row, column;

	public Coordinate(byte row, byte column)
	{
		this.row = row;
		this.column = column;
	}

}
