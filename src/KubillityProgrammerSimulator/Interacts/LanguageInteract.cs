using KubillityProgrammerSimulator.Models;
using KubillityProgrammerSimulator.Prompts;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class LanguageInteract
    {
        private DoublePrompt doublePrompt = new DoublePrompt(10, 100);
        private OptionPrompt optionPrompt = new OptionPrompt("请选择工作语言，可设置多个工作语言：", 8);

        public void Invoke()
        {
            var languages = new Dictionary<Language, double>();
            var choices = new List<string> { "选择完毕" };
            foreach (var item in Enum.GetValues(typeof(Language)))
            {
                choices.Add(item.ToString());
            }
            while (true)
            {
                var language = optionPrompt.Ask(choices.ToArray());
                if (language == "选择完毕")
                {
                    if (!languages.Any())
                    {
                        AnsiConsole.MarkupLine("[red]请至少选择一种工作语言[/]");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    var ratio = this.doublePrompt.Ask($"请设置 {language} 在工作中的使用占比：");
                    languages.Add((Language)Enum.Parse(typeof(Language), language), ratio);
                }
            }

            var lead = Game.Instance.GetLead();
            lead!.Languages = languages.Keys.ToList();
            new InductionInteract().Invoke();
        }
    }
}
