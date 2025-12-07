using var reader = new StreamReader("input.txt");

var ranges = new List<(long start, long end)>();
var ids = new List<long>();
var startIds = false;
var result = 0;
while (!reader.EndOfStream) {
    var lineStr = reader.ReadLine();
    if (string.IsNullOrWhiteSpace(lineStr?.Trim())) {
        startIds = true;
    } else {
        if (startIds) {
            var id = long.Parse(lineStr!);
            foreach (var (start, end) in ranges) {
                if (id >= start && id <= end) {
                    result++;
                    break;
                }
            }
        } else {
            var parts = lineStr!.Split('-');
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);
            ranges.Add((start, end));
        }
    }
}

Console.WriteLine(result);