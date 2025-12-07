using var reader = new StreamReader("input.txt");
long result = 0;
var grid = new List<List<string>>();
while (!reader.EndOfStream)
{
    var lineStr = reader.ReadLine();
    var line = lineStr!.Trim().Split(' ').Where(s=>!string.IsNullOrWhiteSpace(s)).ToList();
    grid.Add(line);
}

for (var col = 0; col < grid[0].Count; col++)
{
    var op = grid[grid.Count - 1][col];
    long lineResult = long.Parse(grid[0][col]);
    for (var row = 1; row < grid.Count - 1; row++)
    {
        if (op == "+")
        {
            lineResult += long.Parse(grid[row][col]);
        }
        else if (op == "*")
        {
            lineResult *= long.Parse(grid[row][col]);
        }
    }
    result += lineResult;
}

Console.WriteLine(result);