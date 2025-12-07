using System.Runtime.CompilerServices;

using var reader = new StreamReader("input.txt");
long result = 0;
var grid = new List<char[]>();
while (!reader.EndOfStream) {
    var lineStr = reader.ReadLine();
    var line = lineStr!.ToCharArray();
    grid.Add(line);
}


var numbers = new List<long>();

var op = '0';
for (var col = 0; col < grid[0].Length; col++) {
    if (op == '0') {
        for (var colAux = col; colAux < grid[^1].Length; colAux++) {
            if (grid[^1][colAux] != ' ') {
                op = grid[^1][colAux];
                break;
            }
        }
    }

    var number = new List<char>();
    for (var row = 0; row < grid.Count - 1; row++) {
        if (grid[row][col] != ' ') {
            number.Add(grid[row][col]);
        }
    }
    if (number.Count > 0) {
        numbers.Add(long.Parse(new string(number.ToArray())));
    } else if (numbers.Count > 0) {
        Calc();
        op = '0';
        numbers.Clear();

    }
}
if (numbers.Count > 0) {
    Calc();
}

void Calc() {
    long colResult = numbers[0];
    for (var j = 1; j < numbers.Count; j++) {
        if (op == '+') {
            colResult += numbers[j];
        } else {
            colResult *= numbers[j];
        }
    }
    result += colResult;
}

Console.WriteLine(result);
//11494432218296
//11494432218296
//11494432218296