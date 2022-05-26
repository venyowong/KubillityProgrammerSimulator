using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class TimePassInteract
    {
        public void Invoke()
        {
            AnsiConsole.MarkupLine($"[grey]Day {Time.Instance.Day} {Time.Instance.Current}[/]");
            while (true)
            {
                Game.Instance.TimePassed();
                Time.Instance.Pass(60);
                AnsiConsole.MarkupLine($"[grey]Day {Time.Instance.Day} {Time.Instance.Current}[/]");
            }
        }
    }
}
