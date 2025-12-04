using var reader = new StreamReader("input.txt");

var result = 0;
while (!reader.EndOfStream) {
    var bank = reader.ReadLine();
    var firstNumber = -1;
    var secondNumber = -1;
    for (var i = 0; i < bank?.Length; i++) {
        var number = int.Parse(bank.Substring(i, 1));
        if (number > firstNumber && i < bank.Length - 1) {
            secondNumber = -1;
            firstNumber = number;
        } else if (number > secondNumber) {
            secondNumber = number;
        }
    }
    result += int.Parse(firstNumber.ToString() + secondNumber.ToString());
}

Console.WriteLine(result);
