using Console;
using System.Threading.Tasks;

public class MyCustomCommands
{
    [ConsoleCommand("hello", "test")]
    public class HelloCommand : Command
    {
        [CommandParameter("name")]
        public string Name;
        public override ConsoleOutput Logic()
        {
            return new ConsoleOutput($"Hello {Name}!", ConsoleOutput.OutputType.Log);
        }
    }

    [ConsoleCommand("joinrelay", "join relay (client)")]
    public class JoinRelayCommand : Command
    {
        [CommandParameter("join code")]
        public string JoinCode;
        public override ConsoleOutput Logic()
        {
            if(string.IsNullOrWhiteSpace(JoinCode)) return new ConsoleOutput("invalid join code", ConsoleOutput.OutputType.Error);

            GameManager.Instance.JoinRelayAsync(JoinCode);
            return new ConsoleOutput("joining...", ConsoleOutput.OutputType.Log);
        }
    }

    [ConsoleCommand("hostrelay", "host relay (host)")]
    public class HostRelayCommand : Command
    {
        public override ConsoleOutput Logic()
        {
            GameManager.Instance.HostRelayAsync();
            return new ConsoleOutput("hosting...", ConsoleOutput.OutputType.Log);
        }
    }
}
