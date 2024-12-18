public class Maze {
    public const char WALL = '#';
    public const char START = 'S';
    public const char END = 'E';

    private List<List<char>> _puzzle;

    public static readonly List<Movement> POSSIBLE_MOVEMENTS = new(){
        Movement.Left,
        Movement.Top,
        Movement.Right,
        Movement.Bottom
    };

    public Maze(List<List<char>> puzzle) {
        _puzzle = puzzle;
    }

    public long CalcLowestScore() {
        //Find start position
        MazeNode? startNode = null;
        for (var line = _puzzle.Count - 1; line >= 0; line--) {
            for (var column = 0; column < _puzzle[line].Count - 1; column++) {
                if (_puzzle[line][column] == START) {
                    startNode = new MazeNode(this, new MazeCoordinate(line, column), true, Movement.Right);
                }
            }
        }
        if (startNode is null) {
            Console.WriteLine("Startpoint not found");
            return -1;
        }
        return startNode.CalcLowestScore(0, new MazePath());
    }

    public char GetChar(MazeCoordinate coordinate) {
        if ((coordinate.Column < _puzzle[0].Count) && (coordinate.Line < _puzzle.Count) &&
            (coordinate.Column >= 0) && (coordinate.Line >= 0)) {
            return _puzzle[coordinate.Line][coordinate.Column];
        }

        return WALL;
    }
}



public class MazeNode {

    private bool _isRoot;
    private Maze _maze;
    private MazeCoordinate _coordinate;
    private Movement _currentDirection;
    // private List<MazeNode> _children = new();

    private const int CHANGE_DIRECTION = 1001;
    private const int KEEP_MOVEMENT_SCORE = 1;

    public MazeNode(Maze maze, MazeCoordinate coordinate, bool isRootNode, Movement direction) {
        _maze = maze;
        _coordinate = coordinate;
        _isRoot = isRootNode;
        _currentDirection = direction;
    }

    public long CalcLowestScore(long currentScore, MazePath mazePath) {
        var content = _maze.GetChar(_coordinate);
        switch (content) {
            case Maze.END: {
                    return currentScore;
                }
            case Maze.WALL: {
                    return -1;
                }
        }

        long lowestScore = long.MaxValue;

        foreach (var movement in Maze.POSSIBLE_MOVEMENTS) {
            //nao pode fazer 180 graus de movimento pq estaria voltando
            if ((movement != _currentDirection) && (Math.Abs(movement - _currentDirection) != 1)) {
                continue;
            }
            long score = currentScore + (_currentDirection == movement ? KEEP_MOVEMENT_SCORE : CHANGE_DIRECTION);
            var nextCoordinate = new MazeCoordinate(_coordinate.Line, _coordinate.Column);
            switch (movement) {
                case Movement.Left: {
                        nextCoordinate.Column--;
                        break;
                    }
                case Movement.Right: {
                        nextCoordinate.Column++;
                        break;
                    }
                case Movement.Top: {
                        nextCoordinate.Line--;
                        break;
                    }
                case Movement.Bottom: {
                        nextCoordinate.Line++;
                        break;
                    }
            }
            mazePath.SaveCheckPoint();
            try {
                if (mazePath.AddNewNodeCoordinate(nextCoordinate)) {
                    var node = new MazeNode(_maze, nextCoordinate, false, movement);
                    score = node.CalcLowestScore(score, mazePath);
                    if ((score != -1) && (score < lowestScore)) {
                        lowestScore = score;
                    }
                }
            }
            finally {
                mazePath.RestoreCheckPoint();
            }
        }
        return lowestScore != long.MaxValue ? lowestScore : -1;
    }


}