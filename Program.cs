using static System.Console;

class Magic
{
    static async Task<int> Main()
    {
        var program = new Program();

        await program.Run();

        return 0;
    }
}

class Program
{
    readonly List<string> _context = new List<string>();
    int _iteration = 0;

    public async Task Run()
    {
        await foreach (var prompt in Prompt())
        {
            if (!await Act(prompt))
            {
                break;
            }
        }
    }

    async IAsyncEnumerable<Action> Prompt()
    {
        while (true)
        {
            if (_iteration == 0)
            {
                await Help();
            }
            _iteration++;

            Write("?? ");

            var cki = ReadKey();

            WriteLine();

            switch (cki.Key)
            {
                case ConsoleKey.D1:
                    yield return Action.Request;
                    break;
                case ConsoleKey.D2:
                    yield return Action.Reset;
                    break;
                case ConsoleKey.H:
                    yield return Action.Help;
                    break;
                case ConsoleKey.Q:
                    yield return Action.Exit;
                    break;
                default:
                    yield return Action.Invalid;
                    break;
            }
        }
    }

    async Task<bool> Act(Action action)
    {
        var actionMap = new Dictionary<Action, Func<Task>> {
            { Action.Request, Request },
            { Action.Reset, Reset },
            { Action.Help, Help },
            { Action.Invalid, async () => { WriteLine("!! Invalid key"); await Task.CompletedTask; } },
        };
        switch (action)
        {
            case Action.Exit:
                return false;
            default:
                await actionMap[action]();
                return true;
        }
    }

    async Task Request()
    {
        var input = new System.Text.StringBuilder();

        if (_context.Count == 0)
        {
            WriteLine(".<return> to end request");
        }

        WriteLine(">>");

        while (true)
        {
            var line = ReadLine();
            if (line == null || line.Equals("."))
            {
                break;
            }
            else
            {
                input.AppendLine(line);
            }
        }

        var trimmedInput = input.ToString().Trim();

        if (trimmedInput.Length < 8)
        {
            return;
        }
        WriteLine("## Thinking...");

        var result = await GPT3.GPT3Api.Completions(new GPT3.Request { Prompt = trimmedInput });

        if (result.Choices.Any())
        {
            var response = result.Choices.First().Text.Trim().Split("\n");

            WriteLine("<<");
            foreach (var line in response)
            {
                WriteLine(line);
            }
        }
        else
        {
            WriteLine("!! Invalid result");
            WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }

        _context.Add(trimmedInput);
    }

    async Task Reset()
    {
        _context.Clear();
        WriteLine("!! Context reset");

        await Task.Yield();
    }

    async Task Help()
    {
        WriteLine("?? 1 = Request, 2 = Reset, H = Help, Q = Exit (or ctrl+c)"); ;

        await Task.Yield();
    }
}

enum Action
{
    Request,
    Reset,
    Exit,
    Help,
    Invalid = 99
}
