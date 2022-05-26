using KubillityProgrammerSimulator.Flows;
using KubillityProgrammerSimulator.Helpers;
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
    public class DoWorkInteract
    {
        private DoublePrompt prompt = new DoublePrompt(10, 1440);

        public void Invoke()
        {
            var minutes = this.prompt.Ask("打算写多久代码？以分钟为单位：");
            var lead = Game.Instance.GetLead();
            var company = Game.Instance.GetCompany(lead!.Company);
            var tasks = lead.RunningTasks.Select(t => company!.GetRunningTask(t)).ToList();
            var option = "维护老项目";
            if (tasks.Any())
            {
                var choices = new List<string> { "维护老项目" };
                choices.AddRange(tasks.Select(t => t!.Description));
                option = new OptionPrompt("选择想做的工作内容：", 8)
                    .Ask(choices.ToArray());
            }

            if (option == "维护老项目")
            {
                var aside = Game.Instance.GetAside("DoWork");
                var flow = new MultilineTextFlow(500);
                foreach (var text in aside!.Parse())
                {
                    flow.AddText(text);
                }
                flow.AddText($"[yellow]翻看了 {minutes} 分钟老代码之后...[/]")
                    .Show();
            }
            else
            {
                minutes = tasks.First(t => t!.Description == option)!.Handle(minutes);
            }
            minutes = Time.Instance.Pass(minutes);

            lead!.WeeklyVitality -= minutes / 60.0;
            AnsiConsole.MarkupLine($"[grey]Day {Time.Instance.Day} {Time.Instance.Current}[/]");
            Game.Instance.TimePassed();
        }
    }
}
