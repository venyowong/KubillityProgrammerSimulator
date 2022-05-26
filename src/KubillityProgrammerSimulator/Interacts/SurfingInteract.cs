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
    public class SurfingInteract
    {
        private DoublePrompt prompt = new DoublePrompt(10, 1440);

        public void Invoke()
        {
            var minutes = this.prompt.Ask("打算刷多久网页？以分钟为单位：");
            minutes = Time.Instance.Pass(minutes);
            var aside = Game.Instance.GetAside("Surfing");
            var flow = new MultilineTextFlow(500);
            foreach (var text in aside!.Parse())
            {
                flow.AddText(text);
            }
            flow.AddText($"[yellow]在网上冲浪时间过得就是快，{minutes} 分钟过去了...[/]")
                .Show();
            var lead = Game.Instance.GetLead();
            lead!.WeeklyVitality += 0.25 * minutes / 60; // 发呆一小时恢复 0.25 活力
            if (lead.WeeklyVitality > 40)
            {
                lead.WeeklyVitality = 40;
            }
            AnsiConsole.MarkupLine($"[grey]Day {Time.Instance.Day} {Time.Instance.Current}[/]");
            Game.Instance.TimePassed();
        }
    }
}
