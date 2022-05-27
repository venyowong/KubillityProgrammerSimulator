using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class ResumeInteract
    {
        public void Invoke()
        {
            AnsiConsole.MarkupLine("游戏继续...");

            var lead = Game.Instance.GetLead();
            if (string.IsNullOrWhiteSpace(lead!.Name))
            {
                new NameInteract().Invoke();
                return;
            }
            if (!lead.Languages.Any())
            {
                new LanguageInteract().Invoke();
                return;
            }
            if (lead.Company < 0)
            {
                new InductionInteract().Invoke();
                return;
            }
            if (lead.Goal < 200000)
            {
                new GoalInteract().Invoke();
            }

            new TimePassInteract().Invoke();
        }
    }
}
