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
    public class StareBlanklyInteract
    {
        private DoublePrompt prompt = new DoublePrompt(10, 1440);

        public void Invoke()
        {
            var minutes = this.prompt.Ask("打算发呆多久？以分钟为单位：");
            minutes = Time.Instance.Pass(minutes);
            var aside = Game.Instance.GetAside("StareBlankly");
            var flow = new MultilineTextFlow(500);
            foreach (var text in aside!.Parse())
            {
                flow.AddText(text);
            }
            flow.AddText($"[yellow]看了 {minutes} 分钟后，眼睛都酸胀了...[/]")
                .Show();
            var lead = Game.Instance.GetLead();
            lead!.WeeklyVitality += 0.2 * minutes / 60; // 发呆一小时恢复 0.2 活力
            if (lead.WeeklyVitality > 40)
            {
                lead.WeeklyVitality = 40;
            }
            AnsiConsole.MarkupLine($"[grey]Day {Time.Instance.Day} {Time.Instance.Current}[/]");
            Game.Instance.TimePassed();
        }
    }
}
