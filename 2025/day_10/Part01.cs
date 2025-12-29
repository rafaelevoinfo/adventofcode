using var reader = new StreamReader("input.txt");
int result = 0;
var machines = new List<Machine>();


while (!reader.EndOfStream)
{
    var lineStr = reader.ReadLine();
    var line = lineStr!.Split(' ');

    var goalBinary = line[0].Replace('.', '0').Replace('#', '1')[1..^1];
    var goal = Convert.ToInt32(goalBinary, 2);
    machines.Add(new Machine
    {
        Goal = goal,
        Buttons = line.Skip(1).SkipLast(1).Select(s => IndicesStringToBitmask(s, goalBinary.Length)).ToList()
    });
}

foreach (var machine in machines)
{
    result += FindLowerPath(machine);
}

Console.WriteLine(result);


static int FindLowerPath(Machine machine)
{
    var visistedStates = new HashSet<State>();
    var states = new Queue<State>();

    foreach (var button in machine.Buttons)
    {
        if (button == machine.Goal)
        {
            return 1;
        }
        states.Enqueue(new State()
        {
            CurrentState = 0 ^ button,
            ButtonUsed = button,
            Level = 1
        });
    }
    while (states.Count > 0)
    {
        var state = states.Dequeue();
        foreach (var button in machine.Buttons)
        {
            if (button != state.ButtonUsed)
            {
                var newState = new State()
                {
                    CurrentState = state.CurrentState ^ button,
                    ButtonUsed = button,
                    Level = state.Level + 1
                };
                if (newState.CurrentState == machine.Goal)
                {
                    return newState.Level;
                }
                if (!visistedStates.Contains(newState))
                {
                    states.Enqueue(newState);
                }
            }
        }
    }
    return -1;
}

static int IndicesToBitmask(IEnumerable<int> indices, int width)
{
    int mask = 0;
    foreach (var i in indices)
    {
        if (i >= 0 && i < width)
        {
            int pos = width - 1 - i; // index counted from left (MSB)
            mask |= (1 << pos);
        }
    }
    return mask;
}

static int IndicesStringToBitmask(string s, int width)
{
    if (string.IsNullOrWhiteSpace(s)) return 0;
    if (s.StartsWith("(") && s.EndsWith(")")) s = s[1..^1];
    var parts = s.Split(',', StringSplitOptions.RemoveEmptyEntries);
    var nums = new List<int>();
    foreach (var p in parts)
    {
        if (int.TryParse(p.Trim(), out var n)) nums.Add(n);
    }
    return IndicesToBitmask(nums, width);
}

public record State
{
    public int CurrentState { get; set; }
    public int ButtonUsed { get; set; }
    public int Level { get; set; }
}

public record Machine
{
    public int Goal { get; init; }
    public List<int> Buttons { get; init; } = new();
}