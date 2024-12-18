using System.Text;




public class MazeParser {
    public static Maze Parse(string? file) {
        // var dir = Directory.GetCurrentDirectory();
        // file = "..\\..\\..\\maze_sample.txt";
        if (!File.Exists(file)) {
            throw new FileNotFoundException("File not found");
        }
        var result = new List<List<char>>();
        var lines = File.ReadAllLines(file);
        for (var i = 0; i < lines.Length; i++) {
            result.Add(new List<char>());
            var charArray = lines[i].ToCharArray();
            for (var j = 0; j < charArray.Length; j++) {
                result[i].Add(charArray[j]);
            }
        }
        return new Maze(result);


        // var fileStream = new FileStream(file, FileMode.Open);
        // var buffer = new byte[1024];
        // var bytesRead = 0;
        // var lines = 0;
        // var currentLine = 0;
        // while ((bytesRead = fileStream.Read(buffer)) > 0)
        // {
        //     var content = Encoding.UTF8.GetString(buffer);
        //     for (var i = 0; i < content.Length; i++)
        //     {
        //         if ()
        //     }
        // }

    }
}