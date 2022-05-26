using KubillityProgrammerSimulator.Commands;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Prompts
{
    public class DoublePrompt
    {
        private double min;
        private double max;
        private ICommand generalCommands = new GeneralCommands();

        public DoublePrompt(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public double Ask(string tip, Style? style = null)
        {
            if (style == null)
            {
                style = Style.Plain;
            }

            double result = 0;
            while (true)
            {
                var text = AnsiConsole.Prompt(
                    new TextPrompt<string>(tip)
                        .PromptStyle(style)
                        .ValidationErrorMessage(string.Empty)
                        .Validate(input =>
                        {
                            if (string.IsNullOrEmpty(input))
                            {
                                return false;
                            }
                            if (input.StartsWith('/'))
                            {
                                return true;
                            }
                            if (!double.TryParse(input, out result))
                            {
                                AnsiConsole.Markup($"[red]请输入合法数字[/]");
                                return false;
                            }
                            if (result < this.min)
                            {
                                AnsiConsole.Markup($"[red]输入数字必须大于等于 {this.min}[/]");
                                return false;
                            }
                            if (result > this.max)
                            {
                                AnsiConsole.Markup($"[red]输入数字必须小于等于 {this.max}[/]");
                                return false;
                            }

                            return true;
                        }));

                if (text.StartsWith('/'))
                {
                    generalCommands.Execute(text);
                }
                else
                {
                    return result;
                }
            }
        }
    }
}
