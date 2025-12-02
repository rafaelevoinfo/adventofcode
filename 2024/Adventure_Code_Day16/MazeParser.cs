public class MazeParser {
    public static MazeWithPriorityQueue Parse(string? file) {
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
        return new MazeWithPriorityQueue(result);
    }
}