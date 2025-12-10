using var reader = new StreamReader("input.txt");

var columnsInfo = new Dictionary<int, long>();
var grid = new List<List<char>>();
long result = 0;
while (!reader.EndOfStream) {
    var lineStr = reader.ReadLine();
    grid.Add(lineStr!.ToList());
}

for (var col = 0; col < grid[0].Count; col++) {
    if (grid[0][col] == 'S') {
        columnsInfo.Add(col, 1);
        break;
    }
}
for (var row = 1; row < grid.Count; row++) {
    var newColumnsInfo = new Dictionary<int, long>();
    foreach (var col in columnsInfo) {
        if (grid[row][col.Key] == '^') {
            var newColumnLeft = col.Key - 1;
            var newColumnRight = col.Key + 1;

            if (!newColumnsInfo.TryAdd(newColumnLeft, col.Value)) {
                newColumnsInfo[newColumnLeft] += col.Value;
            }
            if (!newColumnsInfo.TryAdd(newColumnRight, col.Value)) {
                newColumnsInfo[newColumnRight] += col.Value;
            }
        } else {
            if (!newColumnsInfo.TryAdd(col.Key, col.Value)) {
                newColumnsInfo[col.Key] += col.Value;
            }
        }
    }
    if (newColumnsInfo.Count > 0) {
        columnsInfo = newColumnsInfo;
    }
}

foreach (var col in columnsInfo) {
    result += col.Value;
}

Console.WriteLine(result);