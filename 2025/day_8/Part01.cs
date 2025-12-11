using var reader = new StreamReader("input.txt");

var circuits = new List<Circuit>();
var distances = new PriorityQueue<(int, int), double>();
var junctionBoxes = new List<JunctionBox>();

while (!reader.EndOfStream) {
    var lineStr = reader.ReadLine();
    var parts = lineStr!.Split(',', StringSplitOptions.TrimEntries);
    junctionBoxes.Add(new JunctionBox(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
}

for (var i = 0; i < junctionBoxes.Count; i++) {
    for (var j = i + 1; j < junctionBoxes.Count; j++) {
        var distance = CalcularDistanciaEntrePontos(junctionBoxes[i], junctionBoxes[j]);
        distances.Enqueue((i, j), distance);
    }
}

int connections = 0;
while (distances.Count > 0 && connections < 1000) {
    var (boxA, boxB) = distances.Dequeue();

    Circuit? circuitA = circuits.FirstOrDefault(c => c.JunctionBoxes.Contains(boxA));
    Circuit? circuitB = circuits.FirstOrDefault(c => c.JunctionBoxes.Contains(boxB));

    if (circuitA is null && circuitB is null) {
        circuits.Add(new Circuit { JunctionBoxes = [boxA, boxB] });
    } else if (circuitA is not null && circuitB is not null && circuitA != circuitB) {
        circuitA.JunctionBoxes.UnionWith(circuitB.JunctionBoxes);
        circuitB.JunctionBoxes.Clear();
        circuits.Remove(circuitB);
    } else if (circuitA is not null && circuitB is null) {
        circuitA.JunctionBoxes.Add(boxB);
    } else if (circuitB is not null && circuitA is null) {
        circuitB.JunctionBoxes.Add(boxA);
    }
    connections++;

}

var result = circuits.Select(c => c.JunctionBoxes.Count)
    .OrderByDescending(s => s)
    .Take(3)
    .Aggregate(1, (acc, size) => acc * size);

Console.WriteLine(result);

double CalcularDistanciaEntrePontos(JunctionBox a, JunctionBox b) {
    //elevar ao quadrado a diferenca de cada coordenada
    return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
}

record JunctionBox(int X, int Y, int Z);
class Circuit {
    public HashSet<int> JunctionBoxes { get; init; } = [];
}