// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Input the maze file");
var mazeFile = Console.ReadLine();

var maze = MazeParser.Parse(mazeFile);
var timer = Stopwatch.StartNew();
try
{
    var lowestScore = maze.CalcLowestScore();
    timer.Stop();
    if (lowestScore <= 0)
    {
        Console.WriteLine($"It was not possible to calculate the lowest score. Sorry :(");
    }
    else
    {
        Console.WriteLine($"The lowest score is {lowestScore}. Time: {timer.ElapsedMilliseconds} ms");
        timer = Stopwatch.StartNew();
        var count = maze.CountPartsOfBestPaths();
        timer.Stop();
        Console.WriteLine($"The number of parts of the best paths is {count}. Time: {timer.ElapsedMilliseconds} ms");
    }

}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

// Console.ReadKey();
