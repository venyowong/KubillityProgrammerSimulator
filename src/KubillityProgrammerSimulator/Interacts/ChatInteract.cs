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
    public class ChatInteract
    {
        private DoublePrompt prompt = new DoublePrompt(10, 1440);

        public void Invoke()
        {
            var minutes = this.prompt.Ask("打算聊多久？以分钟为单位：");
            minutes = Game.Instance.GetKeyTime(minutes);
            var lead = Game.Instance.GetLead();
            var aside = Game.Instance.GetAside("Chat");
            var flow = new MultilineTextFlow(500);
            foreach (var text in aside!.Parse())
            {
                flow.AddText(text);
            }

            var random = new Random((int)DateTime.Now.Ticks);
            List<Influence>? influences = null;
            if (random.NextDouble() <= 0.1 * minutes / 120) // 聊天两小时触发事件概率 10%
            {
                var plot = Game.Instance.GetPlot("Chat");
                if (plot != null)
                {
                    influences = plot.Influences;
                    if (lead!.MatchConditions(plot.Conditions))
                    {
                        minutes *= 0.3 + 0.7 * random.NextDouble(); // 聊天中途发生事件
                        minutes = Math.Floor(minutes);
                        flow.AddText("[yellow]新事件发生...[/]");
                        foreach (var text in plot.Content.Parse())
                        {
                            flow.AddText(text);
                        }
                    }
                }
            }

            minutes = Time.Instance.Pass(minutes);
            flow.AddText($"[yellow]扯了 {minutes} 分钟后...[/]")
                .Show();
            if (influences != null)
            {
                lead!.BeAffected(influences);
            }
            lead!.WeeklyVitality += 0.3 * minutes / 60; // 聊天一小时恢复 0.3 活力
            if (lead.WeeklyVitality > 40)
            {
                lead.WeeklyVitality = 40;
            }
            AnsiConsole.MarkupLine($"[grey]Day{Time.Instance.Day} Week{Time.Instance.Week} No.{Time.Instance.WeekDay} {Time.Instance.Current}[/]");
            Game.Instance.TimePassed();
        }
    }
}
