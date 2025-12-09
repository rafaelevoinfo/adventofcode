using var reader = new StreamReader("input.txt");

var currentColuns = new HashSet<int>();
var grid = new List<List<char>>();
var result = 0;
while (!reader.EndOfStream) {
    var lineStr = reader.ReadLine();
    grid.Add(lineStr!.ToList());
}

for (var col = 0; col < grid[0].Count; col++) {
    if (grid[0][col] == 'S') {
        currentColuns.Add(col);
        break;
    }
}
for (var row = 1; row < grid.Count; row++) {
    var newColumns = new HashSet<int>(currentColuns);
    foreach (var col in currentColuns) {
        if (grid[row][col] == '^') {
            result++;
            newColumns.Remove(col);
            newColumns.Add(col - 1);
            newColumns.Add(col + 1);
        }
    }
    if (newColumns.Count > 0) {
        currentColuns = newColumns;
    }
}

Console.WriteLine(result);