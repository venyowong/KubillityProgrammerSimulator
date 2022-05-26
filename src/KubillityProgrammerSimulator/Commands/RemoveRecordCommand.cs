using KubillityProgrammerSimulator.Prompts;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Commands
{
    public class RemoveRecordCommand : ICommand
    {
        public bool Execute(string commandText)
        {
            if (commandText != "/remove")
            {
                return false;
            }

            var record = new OptionPrompt("请选择要删除的存档：", 8).Ask(Game.Instance.GetRecords().ToArray());
            if (AnsiConsole.Confirm($"确定删除 {record} 存档？"))
            {
                Game.Instance.RemoveRecord(record);
                AnsiConsole.MarkupLine("[green]存档删除成功...[/]");
            }

            return true;
        }
    }
}
