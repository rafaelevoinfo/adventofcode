using var reader = new StreamReader("Src\\input.txt");
var arrowValue = 50;
var count = 0;
while (!reader.EndOfStream) {
    var line = reader.ReadLine();
    if (line?.Length > 0) {
        var direction = line[0];
        if (int.TryParse(line.AsSpan(1), out var clicksNumber)) {
            int newArrowValue = direction switch {
                'L' => arrowValue - clicksNumber,
                'R' => arrowValue + clicksNumber,
                _ => arrowValue
            };

            count += Math.Abs(newArrowValue / 100);

            if (newArrowValue <= 0 && arrowValue > 0) {
                count++;
            }

            newArrowValue %= 100;
            if (newArrowValue < 0) {
                newArrowValue += 100;
            }
            arrowValue = newArrowValue % 100;
        }
    }
}

Console.WriteLine(count);