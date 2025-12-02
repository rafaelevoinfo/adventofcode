public enum Movement
{
    Left,
    Top,
    Right,
    Bottom
}

public static class MovementExtesion
{

    public static bool IsEqualOrEquivalent(this Movement movement, Movement other)
    {
        if (movement == other)
        {
            return true;
        }

        switch (movement)
        {
            case Movement.Left:
                return other == Movement.Right;
            case Movement.Right:
                return other == Movement.Left;
            case Movement.Top:
                return other == Movement.Bottom;
            case Movement.Bottom:
                return other == Movement.Top;
        }

        return false;
    }
}

public class MazeCoordinate
{
    public int Line { get; set; }
    public int Column { get; set; }

    public MazeCoordinate(int line, int column)
    {
        Line = line;
        Column = column;
    }

    public override string ToString()
    {
        return Line.ToString() + "_" + Column.ToString();
    }
}