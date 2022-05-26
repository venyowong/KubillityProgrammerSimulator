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
            // todo: 聊天得到某个消息，触发事件

            var minutes = this.prompt.Ask("打算聊多久？以分钟为单位：");
            minutes = Time.Instance.Pass(minutes);
            var aside = Game.Instance.GetAside("Chat");
            var flow = new MultilineTextFlow(500);
            foreach (var text in aside!.Parse())
            {
                flow.AddText(text);
            }
            flow.AddText($"[yellow]扯了 {minutes} 分钟后...[/]")
                .Show();
            var lead = Game.Instance.GetLead();
            lead!.WeeklyVitality += 0.3 * minutes / 60; // 发呆一小时恢复 0.3 活力
            if (lead.WeeklyVitality > 40)
            {
                lead.WeeklyVitality = 40;
            }
            AnsiConsole.MarkupLine($"[grey]Day {Time.Instance.Day} {Time.Instance.Current}[/]");
            Game.Instance.TimePassed();
        }
    }
}
