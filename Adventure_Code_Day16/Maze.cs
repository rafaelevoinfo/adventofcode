public class Maze {
    private List<List<char>> _puzzle;
    public const char WALL = '#';
    public const char START = 'S';
    public const char END = 'E';
    public Dictionary<string, MazeNode> Nodes = new();
    public const int CHANGE_DIRECTION = 1001;
    public const int KEEP_MOVEMENT_SCORE = 1;
    public static readonly Dictionary<Movement, List<Movement>> ALLOWED_MOVEMENT = new(){
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

    public static readonly Dictionary<Movement, Movement> EQUIVALENT_MOVEMENT = new(){
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
        MazeNode? node = null;

        for (var line = _puzzle.Count - 1; line >= 0; line--) {
            for (var column = 0; column < _puzzle[line].Count - 1; column++) {
                if (_puzzle[line][column] == START) {
                    var coordinate = new MazeCoordinate(line, column);
                    node = new MazeNode(this, coordinate, Movement.Right);

                    Console.WriteLine($"Start Node {coordinate}");
                }
            }
        }

        if (node is null) {
            Console.WriteLine("Startpoint not found");
            return -1;
        }

        var nodesStack = new Stack<MazeNode>();
        long lowestScore = long.MaxValue;
        long score = 0;
        nodesStack.Push(node);
        while (nodesStack.Count > 0) {
            var pop = true;
            if (node.IsEnd) {
                if (score < lowestScore) {
                    lowestScore = score;
                }
            }
            else {

                foreach (var movement in POSSIBLE_MOVEMENTS) {
                    if (ALLOWED_MOVEMENT[node.Direction].Contains(movement) && (!node.Directions[movement])) {
                        var nextNode = GetNextNode(node, movement);
                        if (!nextNode.IsWall && !nextNode.IsStart && !nodesStack.Contains(nextNode)) {
                            node.ScoreToNext = movement == node.Direction ? KEEP_MOVEMENT_SCORE : CHANGE_DIRECTION;
                            if (nextNode.ScoreToEnd[movement] != 0) {
                                var aux = score + nextNode.ScoreToEnd[movement] + node.ScoreToNext;
                                //score += nextNode.ScoreToEnd[movement] + node.ScoreToNext;
                                // if (Maze.EQUIVALENT_MOVEMENT[node.Direction] != movement) {
                                //     score += node.ScoreToEnd[movement] + 1000;
                                // }
                                // else {
                                //     score += node.ScoreToEnd[movement];
                                // }
                                if (aux < lowestScore) {
                                    lowestScore = aux;
                                }
                                continue;
                            }
                            else {
                                node.Directions[movement] = true;
                                if (score + node.ScoreToNext >= lowestScore) {
                                    continue;
                                }
                                score += node.ScoreToNext;
                                nodesStack.Push(nextNode);
                                nextNode.Direction = movement;
                                node = nextNode;
                                pop = false;
                                break;
                            }
                        }

                    }
                }
            }

            if (pop) {
                var oldNode = nodesStack.Pop();
                oldNode.ResetDirections();
                if (nodesStack.Count > 0) {
                    node = nodesStack.Peek();
                    score -= node.ScoreToNext;
                    if (oldNode.IsEnd || (oldNode.ScoreToEnd[oldNode.Direction] != 0)) {
                        //ESSE DIABO AQUI NAO FUNCIONA NAO IMPORTA O QUE FAÇO
                        node.ScoreToEnd[node.Direction] = oldNode.ScoreToEnd[oldNode.Direction] + node.ScoreToNext;
                    }
                    //     node.Directions[node.Direction].Scores[oldNode.Direction].ValidPath = true;
                    // }
                    // else {
                    //     node.Directions[node.Direction].Scores[oldNode.Direction] = oldNode.Directions[oldNode.Direction].ValidPath;
                    // }
                    // node.Directions[node.Direction].Scores[oldNode.Direction].Value = 1;
                }
            }
        }
        return lowestScore;
    }

    private MazeNode GetNextNode(MazeNode currentNode, Movement newDirection) {
        var nextNodeCoordinate = new MazeCoordinate(currentNode.Coordinate.Line, currentNode.Coordinate.Column);
        switch (newDirection) {
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
        //var nextNode = new MazeNode(this, nextNodeCoordinate, newDirection);
        if (!Nodes.TryGetValue(nextNodeCoordinate.ToString(), out var nextNode)) {
            nextNode = new MazeNode(this, nextNodeCoordinate, newDirection);
            Nodes.Add(nextNodeCoordinate.ToString(), nextNode);
        }
        return nextNode;
    }

    public char GetChar(MazeCoordinate coordinate) {
        if ((coordinate.Line < _puzzle.Count) && (coordinate.Column < _puzzle[0].Count) &&
            (coordinate.Column >= 0) && (coordinate.Line >= 0)) {
            return _puzzle[coordinate.Line][coordinate.Column];
        }

        return WALL;
    }
}

public class Score {
    public Movement Direction { get; set; }
    public long Value { get; set; }
}

public class CalculatedScore {
    public Dictionary<Movement, Score> Scores = new(){
        {Movement.Left, new Score()},
        {Movement.Right, new Score()},
        {Movement.Top, new Score()},
        {Movement.Bottom, new Score()}
    };
}



public class MazeNode {
    private Maze _maze;
    public MazeCoordinate Coordinate { get; set; }
    // public Dictionary<Movement, CalculatedScore> Directions = new(){
    //     {Movement.Left, new CalculatedScore()},
    //     {Movement.Right, new CalculatedScore()},
    //     {Movement.Top, new CalculatedScore()},
    //     {Movement.Bottom, new CalculatedScore()}
    // };
    public Dictionary<Movement, bool> Directions = new(){
        {Movement.Left, false},
        {Movement.Right, false},
        {Movement.Top, false},
        {Movement.Bottom, false}
    };

    public Movement Direction { get; set; }
    // public long ScoreToReach { get; set; }
    public Dictionary<Movement, long> ScoreToEnd = new(){
        {Movement.Left, 0},
        {Movement.Right, 0},
        {Movement.Top, 0},
        {Movement.Bottom, 0}
    };
    public long ScoreToNext { get; set; }


    public MazeNode(Maze maze, MazeCoordinate coordinate, Movement direction) {
        _maze = maze;
        Coordinate = coordinate;
        Direction = direction;
    }

    public bool IsWall => _maze.GetChar(Coordinate) == Maze.WALL;
    public bool IsStart => _maze.GetChar(Coordinate) == Maze.START;
    public bool IsEnd => _maze.GetChar(Coordinate) == Maze.END;

    public void ResetDirections() {
        foreach (var direction in Directions.Keys) {
            Directions[direction] = false;
        }
    }

    public override bool Equals(object? obj) {
        return obj is MazeNode node && node.Coordinate.ToString() == Coordinate.ToString();
    }

    public override int GetHashCode() {
        return Coordinate.Line.GetHashCode() + Coordinate.Column.GetHashCode();
    }


}