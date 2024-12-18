// See https://aka.ms/new-console-template for more information
Console.WriteLine("Input the maze file");
var mazeFile = Console.ReadLine();

var maze = MazeParser.Parse(mazeFile);
var lowestScore = maze.CalcLowestScore();
if (lowestScore <= 0) {
    Console.WriteLine($"It was not possible to calculate the lowest score. Sorry :(");
}
else {
    Console.WriteLine($"The lowest score is {lowestScore}");
}

Console.ReadKey();
