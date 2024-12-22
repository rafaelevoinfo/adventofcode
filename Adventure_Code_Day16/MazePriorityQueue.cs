public class MazeWithPriorityQueue {

    public const char WALL = '#';
    public const char START = 'S';
    public const char END = 'E';

    private List<List<char>> puzzle;
    public MazeWithPriorityQueue(List<List<char>> puzzle) {
        this.puzzle = puzzle;
    }

    public long CalcLowestScore() {
        var pq = new PriorityQueue<Node, long>();

        for (var line = puzzle.Count - 1; line >= 0; line--) {
            for (var column = 0; column < puzzle[line].Count - 1; column++) {
                if (puzzle[line][column] == START) {
                    var node = new Node() {
                        Position = new MazeCoordinate(line, column),
                        Score = 0
                    };

                    pq.Enqueue(node, 0);
                }
            }
        }

        var visited = new Dictionary<string, List<Movement>>();
        while (pq.Count > 0) {
            var node = pq.Dequeue();
            if (puzzle[node.Position.Line][node.Position.Column] == END) {
                return node.Score;
            }

            if (visited.TryGetValue(node.Position.ToString(), out var directions) && directions.Contains(node.Direction)) {
                continue;
            }

            if (directions is null) {
                directions = new List<Movement>();
                visited.Add(node.Position.ToString(), directions);
            }
            directions.Add(node.Direction);

            var validMoviments = CalcuteMovements(node.Direction);
            foreach (var m in validMoviments) {
                var nextNodePosition = CalculateNextNodeCoordinate(node, m);
                if (puzzle[nextNodePosition.Line][nextNodePosition.Column] == WALL) {
                    continue;
                }
                var score = node.Score + (m.IsEqualOrEquivalent(node.Direction) ? 1 : 1001);
                pq.Enqueue(new Node() {
                    Position = nextNodePosition,
                    Score = score,
                    Direction = m
                }, score);
            }
        }

        return -1;
    }

    public MazeCoordinate CalculateNextNodeCoordinate(Node currentNode, Movement newDirection) {
        var nextNodeCoordinate = new MazeCoordinate(currentNode.Position.Line, currentNode.Position.Column);
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
        return nextNodeCoordinate;
    }

    public List<Movement> CalcuteMovements(Movement currentDirection) {
        var movements = new List<Movement>();
        movements.Add(currentDirection);
        switch (currentDirection) {
            case Movement.Left:
            case Movement.Right: {
                    movements.Add(Movement.Top);
                    movements.Add(Movement.Bottom);
                    break;
                }
            case Movement.Top:
            case Movement.Bottom: {
                    movements.Add(Movement.Right);
                    movements.Add(Movement.Left);
                    break;
                }
        }
        return movements;
    }
}


public class Node {
    public MazeCoordinate Position { get; set; }
    public int Score { get; set; }
    public Movement Direction { get; set; }
}