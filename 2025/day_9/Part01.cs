using var reader = new StreamReader("input.txt");
var coords = new List<(long Row, long Col)>();

while (!reader.EndOfStream) {
    var lineStr = reader.ReadLine();
    var line = lineStr!.Split(',');
    coords.Add((long.Parse(line[1]), long.Parse(line[0])));
}


long result = 0;
for (var i = 0; i < coords.Count; i++) {
    for (int j = i + 1; j < coords.Count; j++) {
        var area = Math.Max(Math.Abs(coords[j].Row - coords[i].Row) + 1, 1) * Math.Max(Math.Abs(coords[j].Col - coords[i].Col) + 1, 1);
        if (area > result) {
            result = area;
        }
    }
}


Console.WriteLine(result);