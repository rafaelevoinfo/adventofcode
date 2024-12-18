

using System.Collections.Frozen;

public enum Movement {
    Left,
    Top,
    Right,
    Bottom
}

public struct MazeCoordinate {
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
    // public Dictionary<int, List<int>> CurrentPath { get; set; }
    private List<string> _pathTaken = new();
    private Stack<int> _checkPoints = new();

    public bool AddNewNodeCoordinate(MazeCoordinate coordinate) {
        if (!ContainNode(coordinate)) {
            _pathTaken.Add(coordinate.ToString());
            return true;
        }

        return false;
    }

    public long SaveCheckPoint() {
        _checkPoints.Push(_pathTaken.Count);
        return _checkPoints.Last();
    }

    public void RestoreCheckPoint() {
        var index = _checkPoints.Pop();
        _pathTaken.RemoveRange(index, _pathTaken.Count - index);
    }

    private bool ContainNode(MazeCoordinate coordinate) {
        return _pathTaken.Contains(coordinate.ToString());
        // return false;
        // if (CurrentPath.ContainsKey(coordinate.Line)) {
        //     return true;
        // }
        // var columns = CurrentPath[coordinate.Line];
        // if (columns.Contains(coordinate.Column)) {
        //     return true;
        // }
        // return false;


    }

}