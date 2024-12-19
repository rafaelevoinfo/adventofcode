public class Maze {
    private List<List<char>> _puzzle;
    public const char WALL = '#';
    public const char START = 'S';
    public const char END = 'E';
    public Dictionary<string, MazeNode> Nodes = new();
    public static readonly Dictionary<Movement, List<Movement>> AllowedMovements = new(){
        {Movement.Left, new List<Movement>(){Movement.Left, Movement.Top, Movement.Bottom} },
        {Movement.Right, new List<Movement>(){Movement.Right, Movement.Top, Movement.Bottom} },
        {Movement.Bottom, new List<Movement>(){Movement.Bottom, Movement.Left, Movement.Right} },
        {Movement.Top, new List<Movement>(){Movement.Top, Movement.Left, Movement.Right} }
    };

    public static readonly List<Movement> POSSIBLE_MOVEMENTS = new(){
        Movement.Left,
        Movement.Top,
        Movement.Right,
        Movement.Bottom
    };

    public static readonly Dictionary<Movement, Movement> OPOSITE_MOVEMENT = new(){
        {Movement.Left, Movement.Right},
        {Movement.Right, Movement.Left},
        {Movement.Bottom, Movement.Top},
        {Movement.Top, Movement.Bottom}
    };



    public Maze(List<List<char>> puzzle) {
        _puzzle = puzzle;
    }

    public long CalcLowestScore() {
        //Find start position
        MazeNode? startNode = null;
        var mazePath = new MazePath();
        for (var line = _puzzle.Count - 1; line >= 0; line--) {
            for (var column = 0; column < _puzzle[line].Count - 1; column++) {
                if (_puzzle[line][column] == START) {
                    var coordinate = new MazeCoordinate(line, column);
                    startNode = new MazeNode(this, coordinate);
                    mazePath.AddNewNodeCoordinate(coordinate);

                    Console.WriteLine($"Start Node {coordinate}");
                }
            }
        }
        if (startNode is null) {
            Console.WriteLine("Startpoint not found");
            return -1;
        }

        return startNode.CalcLowestScore(mazePath, Movement.Right);
    }

    public char GetChar(MazeCoordinate coordinate) {
        if ((coordinate.Line < _puzzle.Count) && (coordinate.Column < _puzzle[0].Count) &&
            (coordinate.Column >= 0) && (coordinate.Line >= 0)) {
            return _puzzle[coordinate.Line][coordinate.Column];
        }

        return WALL;
    }
}


public class CalculatedScore {
    public Dictionary<Movement, long> Scores = new(){
        {Movement.Left, -2},
        {Movement.Right, -2},
        {Movement.Top, -2},
        {Movement.Bottom, -2}
    };
}



public class MazeNode {
    private Maze _maze;
    private MazeCoordinate _coordinate;
    public Dictionary<Movement, CalculatedScore> Scores = new();

    private const int CHANGE_DIRECTION = 1001;
    private const int KEEP_MOVEMENT_SCORE = 1;

    public MazeNode(Maze maze, MazeCoordinate coordinate) {
        _maze = maze;
        _coordinate = coordinate;
    }

    public long CalcLowestScore(MazePath mazePath, Movement currentDirection) {

        var content = _maze.GetChar(_coordinate);
        if (content == Maze.END) {
            return 0;
        }

        long lowestScore = long.MaxValue;

        foreach (var movement in Maze.POSSIBLE_MOVEMENTS) {
            if (!Maze.AllowedMovements[currentDirection].Contains(movement)) {
                continue;
            }
            long score = currentDirection == movement ? KEEP_MOVEMENT_SCORE : CHANGE_DIRECTION;
            var nextNodeCoordinate = new MazeCoordinate(_coordinate.Line, _coordinate.Column);
            switch (movement) {
                case Movement.Left: {
                        nextNodeCoordinate.Column--;
                        break;
                    }
                case Movement.Right: {
                        nextNodeCoordinate.Column++;
                        break;
                    }
                case Movement.Top: {
                        nextNodeCoordinate.Line--;
                        break;
                    }
                case Movement.Bottom: {
                        nextNodeCoordinate.Line++;
                        break;
                    }
            }

            content = _maze.GetChar(nextNodeCoordinate);
            if (content != Maze.START && content != Maze.WALL) {
                mazePath.SaveCheckPoint();
                try {
                    if (mazePath.AddNewNodeCoordinate(nextNodeCoordinate)) {
                        if (!_maze.Nodes.TryGetValue(nextNodeCoordinate.ToString(), out var node)) {
                            node = new MazeNode(_maze, nextNodeCoordinate);
                            _maze.Nodes.Add(nextNodeCoordinate.ToString(), node);
                        }

                        if (!Scores.TryGetValue(currentDirection, out var scoreNextNode) && !Scores.TryGetValue(Maze.OPOSITE_MOVEMENT[currentDirection], out scoreNextNode)) {
                            scoreNextNode = new CalculatedScore();
                            Scores.Add(currentDirection, scoreNextNode);
                        }

                        long scoreCalculate = scoreNextNode.Scores[movement];
                        if (scoreCalculate == -2) {
                            scoreCalculate = node.CalcLowestScore(mazePath, movement);
                            scoreNextNode.Scores[movement] = scoreCalculate;
                        }

                        if (_coordinate.ToString() == "139_1") {
                            Console.WriteLine($"{movement} - {scoreCalculate}");
                        }


                        if (scoreCalculate >= 0) {
                            score += scoreCalculate;
                            if (score < lowestScore) {
                                lowestScore = score;
                            }
                        }
                    }
                }
                finally {
                    mazePath.RestoreCheckPoint();
                }
            }
        }
        return lowestScore != long.MaxValue ? lowestScore : -1;
    }


}