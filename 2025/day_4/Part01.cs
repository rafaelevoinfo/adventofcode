using var reader = new StreamReader("input.txt");

Main();

void Main() {
    var result = 0;
    var grid = new List<List<short>>();
    var rowIndex = 0;
    while (!reader.EndOfStream) {
        var lineStr = reader.ReadLine();
        grid.Add(new List<short>());
        for (var colIndex = 0; colIndex < lineStr!.Length; colIndex++) {
            grid[rowIndex].Add((short)(lineStr[colIndex] == '@' ? 1 : 0));
            if (rowIndex > 0 && colIndex > 0) {
                if (Check(grid, rowIndex - 1, colIndex - 1) < 4) {
                    result++;
                }
            }
        }
        if (rowIndex > 0) {
            if (Check(grid, rowIndex - 1, lineStr.Length - 1) < 4) {
                result++;
            }
        }

        rowIndex++;
    }

    for (var colIndex = 0; colIndex < grid[rowIndex - 1].Count; colIndex++) {
        if (Check(grid, rowIndex - 1, colIndex) < 4) {
            result++;
        }
    }

    Console.WriteLine(result);
}

int Check(List<List<short>> grid, int rowIndex, int colIndex) {
    if (grid[rowIndex][colIndex] == 0) {
        return 10;
    }
    var minRowIndex = Math.Max(0, rowIndex - 1);
    var maxRowIndex = Math.Min(rowIndex + 1, grid.Count - 1);
    var count = 0;

    for (var r = minRowIndex; r <= maxRowIndex; r++) {
        var minColIndex = Math.Max(0, colIndex - 1);
        var maxColIndex = Math.Min(colIndex + 1, grid[r].Count - 1);
        for (var c = minColIndex; c <= maxColIndex; c++) {
            count += grid[r][c];
        }
    }
    count -= grid[rowIndex][colIndex];
    return count;
}


//1474