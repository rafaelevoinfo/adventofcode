using var reader = new StreamReader("input.txt");

var content = reader.ReadToEnd();
var ranges = content.Split(",");
long total = 0;
foreach (var range in ranges) {
    var pair = range.Split("-");
    long current = long.Parse(pair[0]);
    long end = long.Parse(pair[1]);

    while (current <= end) {
        var curStr = current.ToString();
        var length = curStr.Length;
        var max = curStr.Length / 2;
        for (var i = 1; i <= max; i++) {
            if (length % i == 0) {
                var parts = length / i;
                var chunk = curStr.Substring(0, i);
                var valid = true;
                for (var j = 1; j < parts; j++) {
                    valid = curStr.Substring(j * i, i) == chunk;
                    if (!valid) {
                        break;
                    }
                }
                if (valid) {
                    total += current;
                    break;
                }
            }
        }
        current++;
    }
    //max=3
    //i=2
    //parts=3
    //j=2
    //121212
}

Console.WriteLine(total);
