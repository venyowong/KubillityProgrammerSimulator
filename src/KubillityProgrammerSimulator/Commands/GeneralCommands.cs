using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Commands
{
    public class GeneralCommands : ICommand
    {
        private List<ICommand> commands = new List<ICommand>
        {
            new HelpCommand(),
            new SaveCommand(),
            new RemoveRecordCommand(),
            new InfoCommand()
        };

        public bool Execute(string commandText)
        {
            foreach (var command in this.commands)
            {
                if (command.Execute(commandText))
                {
                    return true;
                }
            }

            AnsiConsole.MarkupLine($"[red]不支持的命令[/]");
            return false;
        }
    }
}
