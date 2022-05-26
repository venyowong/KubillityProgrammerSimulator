using KubillityProgrammerSimulator.Commands;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Prompts
{
    public class OptionPrompt
    {
        private string tip;
        private int pageSize;
        private GeneralCommands generalCommands = new GeneralCommands();

        public OptionPrompt(string tip, int pageSize)
        {
            this.tip = tip;
            this.pageSize = pageSize;
        }

        public string Ask(params string[] choices)
        {
            var cs = new List<string> { "通用命令" };
            cs.AddRange(choices);
            var option = string.Empty;
            while (string.IsNullOrWhiteSpace(option))
            {
                option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title(this.tip)
                        .PageSize(this.pageSize)
                        .MoreChoicesText("[grey](上下键移动以查看更多选项)[/]")
                        .AddChoices(cs.ToArray()));

                if (option == "通用命令")
                {
                    var result = AnsiConsole.Prompt(
                        new TextPrompt<string>("请输入命令(以 / 开始)：")
                            .ValidationErrorMessage(string.Empty)
                            .Validate(input =>
                            {
                                if (string.IsNullOrEmpty(input))
                                {
                                    return false;
                                }
                                if (!input.StartsWith('/'))
                                {
                                    AnsiConsole.Markup("[red]必须以 / 开始，例如 /h[/]");
                                    return false;
                                }
                                if (input.Length < 2)
                                {
                                    AnsiConsole.Markup("[red]输入长度必须大于等于 2[/]");
                                    return false;
                                }
                                if (input.Length > 20)
                                {
                                    AnsiConsole.Markup("[red]输入长度必须小于等于 20[/]");
                                    return false;
                                }

                                return true;
                            }));
                    this.generalCommands.Execute(result);
                    option = string.Empty;
                }
            }
            return option;
        }
    }
}
