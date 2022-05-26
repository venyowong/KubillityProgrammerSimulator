using KubillityProgrammerSimulator.Prompts;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Commands
{
    public class SaveCommand : ICommand
    {
        public bool Execute(string commandText)
        {
            if (commandText != "/save")
            {
                return false;
            }

            var choices = new List<string> { "创建新存档" };
            choices.AddRange(Game.Instance.GetRecords());
            var record = new OptionPrompt("请选择覆盖存档或创建新存档：", 8).Ask(choices.ToArray());
            if (record == "创建新存档")
            {
                var name = AnsiConsole.Prompt(
                new TextPrompt<string>("请输入新存档名称：")
                    .PromptStyle(Style.Plain)
                    .ValidationErrorMessage(string.Empty)
                    .Validate(input =>
                    {
                        if (string.IsNullOrEmpty(input))
                        {
                            return false;
                        }
                        if (input.Length < 5)
                        {
                            AnsiConsole.Markup("[red]输入长度必须大于等于 5[/]");
                            return false;
                        }
                        if (input.Length > 30)
                        {
                            AnsiConsole.Markup("[red]输入长度必须小于等于 30[/]");
                            return false;
                        }

                        return true;
                    }));
                Game.Instance.Save(name);
            }
            else
            {
                Game.Instance.Save(record);
            }

            AnsiConsole.MarkupLine("[green]存档保存成功...[/]");
            return true;
        }
    }
}
