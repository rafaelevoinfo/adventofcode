public class MazeWithPriorityQueue
{

    public const char WALL = '#';
    public const char START = 'S';
    public const char END = 'E';
    private List<List<char>> _puzzle;
    public MazeWithPriorityQueue(List<List<char>> puzzle)
    {
        _puzzle = puzzle;
    }

    public long CalcLowestScore()
    {
        var pq = CreatePriorityQueue();
        var visited = new Dictionary<string, List<Movement>>();
        while (pq.Count > 0)
        {
            var node = pq.Dequeue();
            if (_puzzle[node.Position.Line][node.Position.Column] == END)
            {
                return node.Score;
            }

            if (visited.TryGetValue(node.Position.ToString(), out var directions) && directions.Contains(node.Direction))
            {
                continue;
            }

            if (directions is null)
            {
                directions = new List<Movement>();
                visited.Add(node.Position.ToString(), directions);
            }
            directions.Add(node.Direction);

            var validMoviments = CalcuteMovements(node.Direction);
            foreach (var m in validMoviments)
            {
                var nextNodePosition = CalculateNextNodeCoordinate(node, m);
                if (_puzzle[nextNodePosition.Line][nextNodePosition.Column] == WALL)
                {
                    continue;
                }
                var score = node.Score + (m.IsEqualOrEquivalent(node.Direction) ? 1 : 1001);
                pq.Enqueue(new Node(nextNodePosition)
                {
                    Score = score,
                    Direction = m,
                    Distance = node.Distance + 1
                }, score);
            }
        }

        return -1;
    }

    public int CountPartsOfBestPaths()
    {
        var bestPaths = new HashSet<string>();
        var pq = CreatePriorityQueue();
        var lowestScore = long.MaxValue;

        bestPaths.Add(pq.Peek().Position.ToString());
        var distances = new Dictionary<string, int>();
        while (pq.Count > 0)
        {
            var node = pq.Dequeue();
            if (_puzzle[node.Position.Line][node.Position.Column] == END)
            {
                lowestScore = node.Score;
                while (node.Parent != null)
                {
                    bestPaths.Add(node.Position.ToString());
                    node = node.Parent;
                }

                continue;
            }

            if (distances.TryGetValue(node.Position.ToString(), out var distance) && (node.Distance > distance))
            {
                continue;
            }
            distances[node.Position.ToString()] = node.Distance;

            var validMoviments = CalcuteMovements(node.Direction);
            foreach (var m in validMoviments)
            {
                var nextNodePosition = CalculateNextNodeCoordinate(node, m);
                if (_puzzle[nextNodePosition.Line][nextNodePosition.Column] == WALL)
                {
                    continue;
                }
                var score = node.Score + (m.IsEqualOrEquivalent(node.Direction) ? 1 : 1001);
                if (score > lowestScore)
                {
                    continue;
                }

                pq.Enqueue(new Node(nextNodePosition)
                {
                    Score = score,
                    Parent = node,
                    Direction = m,
                    Distance = node.Distance + 1
                }, score);
            }
        }

        if (bestPaths.Count > 1)
        {
            return bestPaths.Count;
        }
        return -1;
    }

    private PriorityQueue<Node, long> CreatePriorityQueue()
    {
        var pq = new PriorityQueue<Node, long>();

        for (var line = _puzzle.Count - 1; line >= 0; line--)
        {
            for (var column = 0; column < _puzzle[line].Count - 1; column++)
            {
                if (_puzzle[line][column] == START)
                {
                    var node = new Node(new MazeCoordinate(line, column))
                    {
                        Score = 0,
                        Direction = Movement.Right
                    };

                    pq.Enqueue(node, 0);
                    break;
                }

            }
            if (pq.Count > 0)
            {
                break;
            }
        }

        return pq;
    }

    private MazeCoordinate CalculateNextNodeCoordinate(Node currentNode, Movement newDirection)
    {
        var nextNodeCoordinate = new MazeCoordinate(currentNode.Position.Line, currentNode.Position.Column);
        switch (newDirection)
        {
            case Movement.Left:
                {
                    nextNodeCoordinate.Column--;
                    break;
                }
            case Movement.Right:
                {
                    nextNodeCoordinate.Column++;
                    break;
                }
            case Movement.Top:
                {
                    nextNodeCoordinate.Line--;
                    break;
                }
            case Movement.Bottom:
                {
                    nextNodeCoordinate.Line++;
                    break;
                }
        }
        return nextNodeCoordinate;
    }

    private List<Movement> CalcuteMovements(Movement currentDirection)
    {
        var movements = new List<Movement>(){
            currentDirection
        };

        switch (currentDirection)
        {
            case Movement.Left:
            case Movement.Right:
                {
                    movements.Add(Movement.Top);
                    movements.Add(Movement.Bottom);
                    break;
                }
            case Movement.Top:
            case Movement.Bottom:
                {
                    movements.Add(Movement.Right);
                    movements.Add(Movement.Left);
                    break;
                }
        }
        return movements;
    }
}


public class Node
{
    public MazeCoordinate Position { get; set; }
    public int Score { get; set; }
    public Movement Direction { get; set; }
    public Node? Parent { get; set; }
    public int Distance { get; set; }

    public Node(MazeCoordinate position)
    {
        Position = position;
    }
}