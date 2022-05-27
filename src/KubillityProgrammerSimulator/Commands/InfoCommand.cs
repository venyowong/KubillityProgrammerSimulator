using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Commands
{
    public class InfoCommand : ICommand
    {
        public bool Execute(string commandText)
        {
            if (commandText != "/info")
            {
                return false;
            }

            AnsiConsole.MarkupLine($"[grey]Day{Time.Instance.Day} Week{Time.Instance.Week} No.{Time.Instance.WeekDay} {Time.Instance.Current}[/]");
            var lead = Game.Instance.GetLead();
            if (lead != null)
            {
                lead.Report();
            }
            return true;
        }
    }
}
