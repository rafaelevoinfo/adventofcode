using var reader = new StreamReader("input.txt");

long result = 0;
var ranges = new List<(long start, long end)>();
while (!reader.EndOfStream) {
    var lineStr = reader.ReadLine();
    if (string.IsNullOrWhiteSpace(lineStr?.Trim())) {
        break;
    } else {
        var parts = lineStr!.Split('-');
        var start = long.Parse(parts[0]);
        var end = long.Parse(parts[1]);
        ranges.Add((start, end));
    }
}

ranges = ranges.OrderBy(r => r.start).ToList();
var merged = new List<(long start, long end)>() {
    ranges[0]
};
for (var i = 1; i < ranges.Count; i++) {
    var last = merged[^1];
    var current = ranges[i];
    if (current.start <= last.end) {
        merged[^1] = (last.start, Math.Max(last.end, current.end));
    } else {
        merged.Add(current);
    }
}

foreach (var (start, end) in merged) {
    result += (long)(end - start + 1);
}

Console.WriteLine(result);
//277728360343478
//354149806372909
//358180747119019
//363099390341457