public enum Movement {
    Left,
    Top,
    Right,
    Bottom
}

public class MazeCoordinate {
    public int Line { get; set; }
    public int Column { get; set; }

    public MazeCoordinate(int line, int column) {
        Line = line;
        Column = column;
    }

    public override string ToString() {
        return Line.ToString() + "_" + Column.ToString();
    }
}

public class MazePath {
    private List<string> _pathTaken = new();
    private Stack<int> _checkPoints = new();

    public int GetCount() {
        return _pathTaken.Count;
    }

    public bool AddNewNodeCoordinate(MazeCoordinate coordinate) {
        if (!ContainNode(coordinate)) {
            _pathTaken.Add(coordinate.ToString());
            return true;
        }

        return false;
    }

    public void SaveCheckPoint() {
        _checkPoints.Push(_pathTaken.Count);
    }

    public void RestoreCheckPoint() {
        var index = _checkPoints.Pop();
        _pathTaken.RemoveRange(index, _pathTaken.Count - index);
    }

    private bool ContainNode(MazeCoordinate coordinate) {
        return _pathTaken.Contains(coordinate.ToString());
    }

}