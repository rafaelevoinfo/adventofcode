using var reader = new StreamReader("input.txt");

var content = reader.ReadToEnd();
var ranges = content.Split(",");
long total = 0;
foreach (var range in ranges) {
    var pair = range.Split("-");
    long current = long.Parse(pair[0]);
    long end = long.Parse(pair[1]);
    // if ((pair[0].Length % 2) != 0) {
    //     start = long.Parse("1".PadRight(pair[0].Length, '0'));
    // }
    while (current <= end) {
        var curStr = current.ToString();
        if (curStr.Length % 2 == 0) {
            var part1 = curStr.Substring(0, curStr.Length / 2);
            var part2 = curStr.Substring(curStr.Length / 2);
            if (part1 == part2) {
                total += current;
            }
        }
        current++;
    }
}

Console.WriteLine(total);
