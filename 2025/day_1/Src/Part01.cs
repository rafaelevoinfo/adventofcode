using var reader = new StreamReader("Src\\input.txt");
var arrowValue = 50;
var count = 0;
while (!reader.EndOfStream) {
    var line = reader.ReadLine();
    if (line?.Length > 0) {
        var direction = line[0];
        if (int.TryParse(line.AsSpan(1), out var clicksNumber)) {
            switch (direction) {
                case 'L':
                    arrowValue -= clicksNumber;
                    arrowValue %= 100;
                    if (arrowValue < 0) {
                        arrowValue += 100;
                    }
                    break;
                case 'R':
                    arrowValue += clicksNumber;
                    break;
            }
            arrowValue %= 100;

            if (arrowValue == 0) {
                count++;
            }
        }
    }
}
Console.WriteLine("Number of times zero was passed: " + count);

/*
Resposta correta: 1097 
*/