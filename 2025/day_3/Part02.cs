using var reader = new StreamReader("input.txt");

long result = 0;
while (!reader.EndOfStream) {
    var bank = reader.ReadLine();
    var batteriesLength = 12;
    var batteries = new short[batteriesLength];
    for (var i = 0; i < bank?.Length; i++) {
        var number = short.Parse(bank.Substring(i, 1));
        for (var j = 0; j < batteriesLength; j++) {
            if (number > batteries[j] && i <= (bank.Length - (batteriesLength - j))) {
                batteries[j] = number;
                for (var k = j + 1; k < batteriesLength; k++) {
                    batteries[k] = 0;
                }
                break;
            }
        }
    }

    var numberCombined = string.Join("", batteries);
    result += long.Parse(numberCombined);
}

Console.WriteLine(result);
