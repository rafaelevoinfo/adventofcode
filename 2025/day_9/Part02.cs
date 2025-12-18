using var reader = new StreamReader("input.txt");
var vertexs = new List<Vertex>();
var colunsByRow = new Dictionary<long, (long Min, long Max)>();

while (!reader.EndOfStream)
{
    var lineStr = reader.ReadLine();
    var line = lineStr!.Split(',');
    vertexs.Add(new Vertex(long.Parse(line[1]), long.Parse(line[0])));
}

for (int i = 0, j = vertexs.Count - 1; i < vertexs.Count; j = i++)
{
    var minRow = Math.Min(vertexs[i].Row, vertexs[j].Row);
    var maxRow = Math.Max(vertexs[i].Row, vertexs[j].Row);
    var minCol = Math.Min(vertexs[i].Col, vertexs[j].Col);
    var maxCol = Math.Max(vertexs[i].Col, vertexs[j].Col);

    for (var currentRow = minRow; currentRow <= maxRow; currentRow++)
    {
        if (!colunsByRow.TryGetValue(currentRow, out (long MinCol, long MaxCol) limits))
        {
            colunsByRow.Add(currentRow, (minCol, maxCol));
        }
        else
        {
            if (minCol < limits.MinCol)
            {
                colunsByRow[currentRow] = (minCol, limits.MaxCol);
            }
            else if (maxCol > limits.MaxCol)
            {
                colunsByRow[currentRow] = (limits.MinCol, maxCol);
            }
        }
    }
}

long result = 0;
for (var i = 0; i < vertexs.Count; i++)
{
    for (int j = i + 1; j < vertexs.Count; j++)
    {
        var area = Math.Max(Math.Abs(vertexs[j].Row - vertexs[i].Row) + 1, 1) * Math.Max(Math.Abs(vertexs[j].Col - vertexs[i].Col) + 1, 1);
        if (area > result)
        {
            if (CheckRectanguleInsidePolygon(i, j))
            {
                result = area;
            }
        }

    }
}

Console.WriteLine(result);


bool CheckRectanguleInsidePolygon(int i, int j)
{
    var startRow = Math.Min(vertexs[i].Row, vertexs[j].Row);
    var endRow = Math.Max(vertexs[i].Row, vertexs[j].Row);

    for (var currentRow = startRow; currentRow <= endRow; currentRow++)
    {
        var limits = colunsByRow[currentRow];
        var startCol = Math.Min(vertexs[i].Col, vertexs[j].Col);
        var endCol = Math.Max(vertexs[i].Col, vertexs[j].Col);
        if (startCol < limits.Min || endCol > limits.Max)
        {
            return false;
        }
    }

    return true;
}


record Vertex(long Row, long Col);
//incorret answers
//4771508457
//4620005060
//1539809693
//399696255

