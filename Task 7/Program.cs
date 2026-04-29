namespace Task_7;

public class Program
{
    private MyLinkedList? list;
    private bool finished;
    private string message = "Press the specified key to execute command. Outputs of commands will be displayed instead of this message.";

    public static void Main()
    {
        var program = new Program();
        while (!program.finished) program.Poll();
    }

    public void Poll()
    {
        Console.Clear();
        Console.WriteLine(this.message);
        Console.WriteLine(this.list == null ? "No list yet." : $"List contents: {this.list}");
        Console.WriteLine("     -- Available commands --");
        Console.WriteLine("  1: " + ((this.list == null) ? "Create new list" : "Override current list"));
        Console.WriteLine("  2: Prepend value to list");
        Console.WriteLine("  3: Pop index");
        Console.WriteLine("  4: Find first value greater than the input");
        Console.WriteLine("  5: Sum all values less than the input");
        Console.WriteLine("  6: Override the list with values greater than the input");
        Console.WriteLine("  7: Pop everything after the max element");
        Console.WriteLine("  8: Exit");

        var key = Console.ReadKey(true);
        try
        {
            var pollResult = this.MatchKey(key);
            pollResult();
        }
        catch (FormatException)
        {
            this.message = "Invalid format of input.";
        }
        catch (ArgumentNullException)
        {
            this.message = "No list initalized.";
        }
        catch (ArgumentOutOfRangeException)
        {
            this.message = "Index out of range.";
        }
        catch (OverflowException)
        {
            this.message = "Integer overflow. Enter a smaller value.";
        }
    }

    public Action MatchKey(ConsoleKeyInfo key)
    => key.KeyChar switch
    {
        '1' => this.OverrideList,
        '2' => this.PollPrependList,
        '3' => this.PollPop,
        '4' => this.PollFindFirstGreater,
        '5' => this.PollSumLess,
        '6' => this.PollOverrideWithGreater,
        '7' => this.PollPopAfterMax,
        '8' => this.PollExit,
        _ => this.PollUnknownKey,
    };

    public void OverrideList()
    {
        string input = this.GetValidInput("Enter the new list (comma-separated ints).");
        this.list = new MyLinkedList(input.Split(", ").Select(int.Parse));
        this.message = "Created new list.";
    }

    public void PollPrependList()
    {
        ArgumentNullException.ThrowIfNull(this.list);
        string input = this.GetValidInput("Enter the prepended value.");
        int value = int.Parse(input);
        this.list.Prepend(value);
        this.message = $"Prepended value {value}";
    }

    public void PollPop()
    {
        ArgumentNullException.ThrowIfNull(this.list);
        string input = this.GetValidInput("Enter the index to pop.");
        int i = int.Parse(input);
        int poppedValue = this.list[i];
        if (this.list.Length == 1 && i == 0)
        {
            this.list = null;
        }
        else
        {
            this.list.Pop(i);
        }

        this.message = $"Popped value {poppedValue} at index {i}";
    }

    public void PollFindFirstGreater()
    {
        ArgumentNullException.ThrowIfNull(this.list);
        string input = this.GetValidInput("Enter the value to search for.");
        int value = int.Parse(input);
        try
        {
            this.message = $"The first value greater than {value} is {this.list.First(i => i > value)}";
        }
        catch (InvalidOperationException)
        {
            this.message = $"No values greater than {value} found";
        }
    }

    public void PollSumLess()
    {
        ArgumentNullException.ThrowIfNull(this.list);
        string input = this.GetValidInput("Enter the value.");
        int value = int.Parse(input);
        this.message = $"The sum of every value less than {value} is {this.list.Where(i => i < value).Sum()}";
    }

    public void PollOverrideWithGreater()
    {
        ArgumentNullException.ThrowIfNull(this.list);
        string input = this.GetValidInput("Enter the value.");
        int value = int.Parse(input);
        try
        {
            this.list = new MyLinkedList(this.list.Where(i => i > value));
            this.message = "New list successfuly created";
        }
        catch (ArgumentOutOfRangeException)
        {
            this.message = "No elements match specified requirements";
        }
    }

    public void PollPopAfterMax()
    {
        ArgumentNullException.ThrowIfNull(this.list);
        this.list = new MyLinkedList(this.list.Take(this.list.Select((item, i) => new { item, i })
                                                             .First(x => x.item == this.list.Max()).i + 1));
    }

    public void PollExit()
    {
        Console.WriteLine("Exiting...");
        this.finished = true;
    }

    public void PollUnknownKey()
    {
        this.message = "Unknown key pressed. Try again.";
    }

    private string GetValidInput(string message)
    {
        Console.WriteLine(message);
        string? input = Console.ReadLine();
        if (input.IsWhiteSpace() || input == null) throw new FormatException();
        return input;
    }
}
