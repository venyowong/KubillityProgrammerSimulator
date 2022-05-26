using KubillityProgrammerSimulator.Commands;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Prompts
{
    public class StringPrompt
    {
        private bool allowEmpty;
        private int minLength;
        private int maxLength;
        private ICommand generalCommands = new GeneralCommands();

        public StringPrompt(int minLength, int maxLength, bool allowEmpty = false)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
            this.allowEmpty = allowEmpty;
        }

        public string Ask(string tip, Style? style = null)
        {
            if (style == null)
            {
                style = Style.Plain;
            }

            while (true)
            {
                var result = AnsiConsole.Prompt(
                    new TextPrompt<string>(tip)
                        .PromptStyle(style)
                        .ValidationErrorMessage(string.Empty)
                        .Validate(input =>
                        {
                            if (string.IsNullOrEmpty(input))
                            {
                                if (this.allowEmpty)
                                {
                                    return true;
                                }

                                return false;
                            }
                            if (input.Length < this.minLength)
                            {
                                AnsiConsole.Markup($"[red]输入长度必须大于等于 {this.minLength}[/]");
                                return false;
                            }
                            if (input.Length > this.maxLength)
                            {
                                AnsiConsole.Markup($"[red]输入长度必须小于等于 {this.maxLength}[/]");
                                return false;
                            }

                            return true;
                        }));

                if (result.StartsWith('/'))
                {
                    generalCommands.Execute(result);
                }
                else
                {
                    return result;
                }
            }
        }
    }
}
